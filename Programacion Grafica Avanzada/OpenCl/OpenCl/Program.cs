using System;
using System.Diagnostics;
using OpenCL.Net;

class Program
{
    static void Main()
    {
        int N = 10_000_000;

        float[] A = new float[N];
        float[] B = new float[N];
        float[] C = new float[N];

        Random rnd = new Random();

        for (int i = 0; i < N; i++)
        {
            A[i] = (float)rnd.NextDouble();
            B[i] = (float)rnd.NextDouble();
        }


        Stopwatch swCPU = Stopwatch.StartNew();

        for (int i = 0; i < N; i++)
            C[i] = A[i] + B[i];

        swCPU.Stop();
        Console.WriteLine("Tiempo CPU: " + swCPU.ElapsedMilliseconds + " ms");



        string kernelSource = @"
        __kernel void vector_add(
            __global float* A,
            __global float* B,
            __global float* C)
        {
            int i = get_global_id(0);
            C[i] = A[i] + B[i];
        }";

        ErrorCode error;
        
        
        Platform[] platforms = Cl.GetPlatformIDs(out error);
        Device[] devices = Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out error);


        Context context = Cl.CreateContext(
            null,
            1,
            devices,
            null,
            IntPtr.Zero,
            out error);


        CommandQueue queue = Cl.CreateCommandQueue(
            context,
            devices[0],
            (CommandQueueProperties)0,
            out error);


        IMem<float> bufferA = Cl.CreateBuffer<float>(
            context,
            MemFlags.ReadOnly | MemFlags.CopyHostPtr,
            A,
            out error);

        IMem<float> bufferB = Cl.CreateBuffer<float>(
            context,
            MemFlags.ReadOnly | MemFlags.CopyHostPtr,
            B,
            out error);

        IMem<float> bufferC = Cl.CreateBuffer<float>(
            context,
            MemFlags.WriteOnly,
            N,
            out error);


        OpenCL.Net.Program program = Cl.CreateProgramWithSource(
            context,
            1,
            new[] { kernelSource },
            null,
            out error);

        error = Cl.BuildProgram(program, 1, devices, "", null, IntPtr.Zero);

        Kernel kernel = Cl.CreateKernel(program, "vector_add", out error);


        Cl.SetKernelArg(kernel, 0, bufferA);
        Cl.SetKernelArg(kernel, 1, bufferB);
        Cl.SetKernelArg(kernel, 2, bufferC);

        Stopwatch swGPU = Stopwatch.StartNew();

        IntPtr[] workSize = new IntPtr[] { (IntPtr)N };

        Cl.EnqueueNDRangeKernel(
            queue,
            kernel,
            1,
            null,
            workSize,
            null,
            0,
            null,
            out _);

        Cl.Finish(queue);

        Cl.EnqueueReadBuffer(
            queue,
            bufferC,
            Bool.True,
            IntPtr.Zero,
            new IntPtr(N * sizeof(float)),
            C,
            0,
            null,
            out _);

        swGPU.Stop();

        Console.WriteLine("Tiempo GPU OpenCL: " + swGPU.ElapsedMilliseconds + " ms");
    }
}