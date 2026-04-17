using ManagedCuda;
using ManagedCuda.VectorTypes;
using System.Diagnostics;

int N = 10_000_000;

CudaContext ctx = new CudaContext();

CudaKernel kernel = ctx.LoadKernel("VectorAdd.ptx", "VectorAdd");

CudaDeviceVariable<float> d_A = new CudaDeviceVariable<float>(N);
CudaDeviceVariable<float> d_B = new CudaDeviceVariable<float>(N);
CudaDeviceVariable<float> d_C = new CudaDeviceVariable<float>(N);

float[] A = new float[N];
float[] B = new float[N];
float[] C = new float[N];

Random rnd = new Random();

for (int i = 0; i < N; i++)
{
    A[i] = (float)rnd.NextDouble();
    B[i] = (float)rnd.NextDouble();
}

Stopwatch sw = Stopwatch.StartNew();

d_A.CopyToDevice(A);
d_B.CopyToDevice(B);

kernel.BlockDimensions = 256;
kernel.GridDimensions = (N + 255) / 256;

kernel.Run(d_A.DevicePointer, d_B.DevicePointer, d_C.DevicePointer, N);

d_C.CopyToHost(C);

sw.Stop();

Console.WriteLine("Tiempo GPU: " + sw.ElapsedMilliseconds + " ms");
