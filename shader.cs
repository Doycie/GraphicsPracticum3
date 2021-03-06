﻿using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Template_P3
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;

        public int attribute_vpos;
        public int attribute_vnrm;
        public int attribute_vuvs;
        public int uniform_ftime;
        public int uniform_mview;
        public int uniform_model;
        public int uniform_texture;
        public int uniform_campos;
        public int uniform_mnormal;
        public int uniform_skybox;
        public int uniform_diffuse;
        public int uniform_specularity;
        public int uniform_reflection;
        public int uniform_refraction;
        public int uniform_eta;
        public int uniform_lightsAmount;
        public int uniform_lightPos;
        public int uniform_lightColor;
        public int uniform_color;
        public int uniform_ambient;
        public int uniform_vignetting;
        public int uniform_chromatic_abberation;

        // constructor
        public Shader(String vertexShader, String fragmentShader)
        {
            // compile shaders
            programID = GL.CreateProgram();
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            Console.WriteLine(GL.GetProgramInfoLog(programID));

            // get locations of shader parameters
            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            attribute_vnrm = GL.GetAttribLocation(programID, "vNormal");
            attribute_vuvs = GL.GetAttribLocation(programID, "vUV");
            uniform_ftime = GL.GetUniformLocation(programID, "time");
            uniform_mview = GL.GetUniformLocation(programID, "projmatrix");
            uniform_model = GL.GetUniformLocation(programID, "modelmatrix");
            uniform_campos = GL.GetUniformLocation(programID, "campos");
            uniform_mnormal = GL.GetUniformLocation(programID, "normalmatrix");
            uniform_skybox = GL.GetUniformLocation(programID, "skybox");
            uniform_texture = GL.GetUniformLocation(programID, "tex");
            uniform_diffuse = GL.GetUniformLocation(programID, "diffuse");
            uniform_specularity = GL.GetUniformLocation(programID, "specularity");
            uniform_reflection = GL.GetUniformLocation(programID, "reflection");
            uniform_refraction = GL.GetUniformLocation(programID, "refraction");
            uniform_eta = GL.GetUniformLocation(programID, "eta");
            uniform_lightsAmount = GL.GetUniformLocation(programID, "lightsAmount");
            uniform_lightPos = GL.GetUniformLocation(programID, "lightPos");
            uniform_lightColor = GL.GetUniformLocation(programID, "lightColor");
            uniform_color = GL.GetUniformLocation(programID, "color");
            uniform_ambient = GL.GetUniformLocation(programID, "ambient");
            uniform_chromatic_abberation = GL.GetUniformLocation(programID, "chromatic_abberation");
            uniform_vignetting = GL.GetUniformLocation(programID, "vignetting");
        }

        // loading shaders
        private void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            Console.WriteLine("Compiling shader: " +  filename);
            ID = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            Console.WriteLine(GL.GetShaderInfoLog(ID));
        }
    }
}