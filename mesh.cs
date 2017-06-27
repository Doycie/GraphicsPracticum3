﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Template_P3
{
    // mesh and loader based on work by JTalton; http://www.opentk.com/node/642

    public class Mesh
    {
        // data members
        public ObjVertex[] vertices;            // vertex positions, model space

        public ObjTriangle[] triangles;         // triangles (3 vertex indices)
        public ObjQuad[] quads;                 // quads (4 vertex indices)
        int vertexBufferId;                     // vertex buffer
        int triangleBufferId;                   // triangle buffer
        int quadBufferId;                       // quad bufferz

        float diffuse;
        float specularity;
        float refraction;
        float reflection;
        float eta;

        // constructor
        public Mesh(string fileName, float diffuse = 1, float specularity = 0, float reflection = 0, float refraction = 0, float eta = 0)
        {
            MeshLoader loader = new MeshLoader();
            loader.Load(this, fileName);

            this.diffuse = diffuse;
            this.specularity = specularity;
            this.reflection = reflection;
            this.refraction = refraction;
            this.eta = eta;
        }

        // initialization; called during first render
        public void Prepare(Shader shader)
        {
            if (vertexBufferId != 0) return; // already taken care of

            // generate interleaved vertex data (uv/normal/position (total 8 floats) per vertex)
            GL.GenBuffers(1, out vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(ObjVertex))), vertices, BufferUsageHint.StaticDraw);

            // generate triangle index array
            GL.GenBuffers(1, out triangleBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);

            // generate quad index array
            GL.GenBuffers(1, out quadBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
        }

        public void RenderCubeMap(Shader shader, Matrix4 projMatrix, Matrix4 modelmatrix, Texture texture)
        {
            // on first run, prepare buffers
            Prepare(shader);

            GL.UseProgram(shader.programID);
            // enable texture

            if(texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.TextureCubeMap, texture.id);
                int texLoc = GL.GetUniformLocation(shader.programID, "TexCube");
                GL.Uniform1(texLoc, 0);
            }

            // pass transform to vertex shader
            GL.UniformMatrix4(shader.uniform_mview, false, ref projMatrix);
            GL.UniformMatrix4(shader.uniform_model, false, ref modelmatrix);

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);

            // link vertex attributes to shader parameters
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);

            // bind triangle index data and render
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length * 3);

            // bind quad index data and render
            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
                GL.DrawArrays(PrimitiveType.Quads, 0, quads.Length * 4);
            }

            // restore previous OpenGL state
            GL.UseProgram(0);
        }

        // render the mesh using the supplied shader and matrix
        public void RenderSkyReflect(Shader shader, Matrix4 projMatrix, Matrix4 modelmatrix, Texture texture, Texture cubemap, Vector3 c)
        {
            // on first run, prepare buffers
            Prepare(shader);

            GL.UseProgram(shader.programID);

            // enable texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.id);
            int texLoc = GL.GetUniformLocation(shader.programID, "pixels");
            GL.Uniform1(texLoc, 0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemap.id);
            int cubeLoc = GL.GetUniformLocation(shader.programID, "skybox");
            GL.Uniform1(cubeLoc, 1);

            // set material
            GL.Uniform1(shader.uniform_diffuse, diffuse);
            GL.Uniform1(shader.uniform_specularity, specularity);
            GL.Uniform1(shader.uniform_reflection, reflection);
            GL.Uniform1(shader.uniform_refraction, refraction);
            GL.Uniform1(shader.uniform_eta, eta);

            // pass transform to vertex shader
            GL.UniformMatrix4(shader.uniform_mview, false, ref projMatrix);
            GL.UniformMatrix4(shader.uniform_model, false, ref modelmatrix);
            Matrix4 normalMatrix = modelmatrix.Inverted();
            normalMatrix.Transpose();
            GL.UniformMatrix4(shader.uniform_mnormal, false, ref normalMatrix);

            int campos = GL.GetUniformLocation(shader.programID, "campos");
            GL.Uniform3(campos, c.X, c.Y, c.Z);

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);

            // link vertex attributes to shader parameters
            GL.VertexAttribPointer(shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0);
            GL.VertexAttribPointer(shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4);
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);
            GL.EnableVertexAttribArray(shader.attribute_vnrm);
            GL.EnableVertexAttribArray(shader.attribute_vuvs);

            // bind triangle index data and render
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length * 3);

            // bind quad index data and render
            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
                GL.DrawArrays(PrimitiveType.Quads, 0, quads.Length * 4);
            }

            // restore previous OpenGL state
            GL.UseProgram(0);
        }

        public void RenderFur(Shader shader, Matrix4 projMatrix, Matrix4 modelmatrix, Texture texture, float offset)
        {
            // on first run, prepare buffers
            Prepare(shader);

            GL.UseProgram(shader.programID);
            // enable texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.id);
            int texLoc = GL.GetUniformLocation(shader.programID, "pixels");
            GL.Uniform1(texLoc, 0);

            // enable shader

            // pass transform to vertex shader
            GL.UniformMatrix4(shader.uniform_mview, false, ref projMatrix);
            GL.UniformMatrix4(shader.uniform_model, false, ref modelmatrix);
            Matrix4 normalMatrix = modelmatrix.Inverted();
            normalMatrix.Transpose();
            GL.UniformMatrix4(shader.uniform_mnormal, false, ref normalMatrix);

            int offsetID = GL.GetUniformLocation(shader.programID, "shellOffset");
            GL.Uniform1(offsetID, offset);

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);

            // link vertex attributes to shader parameters
            GL.VertexAttribPointer(shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0);
            GL.VertexAttribPointer(shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4);
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);
            GL.EnableVertexAttribArray(shader.attribute_vnrm);
            GL.EnableVertexAttribArray(shader.attribute_vuvs);

            // bind triangle index data and render
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length * 3);

            // bind quad index data and render
            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
                GL.DrawArrays(PrimitiveType.Quads, 0, quads.Length * 4);
            }

            // restore previous OpenGL state
            GL.UseProgram(0);
        }

        public void Render(Shader shader, Matrix4 projMatrix, Matrix4 modelmatrix, Texture texture)
        {
            // on first run, prepare buffers
            Prepare(shader);

            GL.UseProgram(shader.programID);

            if(texture != null)
            {
                // enable texture
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.id);
                GL.Uniform1(shader.uniform_pixels, 0);
            }

            // set material
            GL.Uniform1(shader.uniform_diffuse, diffuse);
            GL.Uniform1(shader.uniform_specularity, specularity);
            GL.Uniform1(shader.uniform_reflection, reflection);
            GL.Uniform1(shader.uniform_refraction, refraction);
            GL.Uniform1(shader.uniform_eta, eta);

            // pass transform to vertex shader
            GL.UniformMatrix4(shader.uniform_mview, false, ref projMatrix);
            GL.UniformMatrix4(shader.uniform_model, false, ref modelmatrix);
            Matrix4 normalMatrix = modelmatrix.Inverted();
            normalMatrix.Transpose();
            GL.UniformMatrix4(shader.uniform_mnormal, false, ref normalMatrix);

            // bind interleaved vertex data
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);

            // link vertex attributes to shader parameters
            GL.VertexAttribPointer(shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0);
            GL.VertexAttribPointer(shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4);
            GL.VertexAttribPointer(shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4);

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray(shader.attribute_vpos);
            GL.EnableVertexAttribArray(shader.attribute_vnrm);
            GL.EnableVertexAttribArray(shader.attribute_vuvs);

            // bind triangle index data and render
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, triangleBufferId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length * 3);

            // bind quad index data and render
            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadBufferId);
                GL.DrawArrays(PrimitiveType.Quads, 0, quads.Length * 4);
            }

            // restore previous OpenGL state
            GL.UseProgram(0);
        }

        // layout of a single vertex
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjVertex
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Vertex;
        }

        // layout of a single triangle
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjTriangle
        {
            public int Index0, Index1, Index2;
        }

        // layout of a single quad
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjQuad
        {
            public int Index0, Index1, Index2, Index3;
        }
    }
} // namespace Template_P3