﻿using OpenTK.Graphics.OpenGL;
using Sledge.Graphics;
using Sledge.DataStructures.Geometric;
using OpenTK;
using Sledge.Graphics.Helpers;
using Matrix = Sledge.Graphics.Helpers.Matrix;

namespace Sledge.UI
{
    public class Viewport3D : ViewportBase
    {
        public Camera Camera { get; set; }

        public Viewport3D()
        {
            Camera = new Camera();
        }

        public Viewport3D(RenderContext context) : base(context)
        {
            Camera = new Camera();
        }

        public override Matrix4 GetViewportMatrix()
        {
            const float near = 0.1f;
            const float far = 50000f;
            var ratio = Width / (float)Height;
            if (ratio <= 0) ratio = 1;
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), ratio, near, far);
        }

        public override Matrix4 GetCameraMatrix()
        {
            var cam = Camera;
            var cameraMatrix = Matrix4.LookAt(
                new Vector3((float)cam.Location.X, (float)cam.Location.Y, (float)cam.Location.Z),
                new Vector3((float)cam.LookAt.X, (float)cam.LookAt.Y, (float)cam.LookAt.Z),
                Vector3.UnitZ);
            return cameraMatrix;
        }

        public ove

        public override void SetViewport()
        {
            base.SetViewport();
            Viewport.Perspective(0, 0, Width, Height);
        }

        protected override void UpdateBeforeClearViewport()
        {
            Camera.Position();
            base.UpdateBeforeClearViewport();
        }

        protected override void UpdateAfterRender()
        {
            base.UpdateAfterRender();
            Listeners.ForEach(x => x.Render3D());

            Matrix.Set(MatrixMode.Modelview);
            Matrix.Identity();
            Viewport.Orthographic(0, 0, Width, Height);
            Listeners.ForEach(x => x.Render2D());
        }

        /// <summary>
        /// Convert a screen space coordinate into a world space coordinate.
        /// The resulting coordinate will be quite a long way from the camera.
        /// </summary>
        /// <param name="screen">The screen coordinate (with Y in OpenGL space)</param>
        /// <returns>The world coordinate</returns>
        public Coordinate ScreenToWorld(Coordinate screen)
        {
            screen = new Coordinate(screen.X, screen.Y, 1);
            var viewport = new[] { 0, 0, Width, Height };= Matrix4d.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), Width / (float)Height, 0.1f, 50000);
            var vm 
            var vm = Matrix4d.LookAt( Vector3d(Camera.Location.X, Camera.Location.Y, Camera.Location.Z),
                new Vector3d(Camera.LookAt.X, Camera.LookAt.Y, Camera.LookAt.Z),
                Vec
                Vector3d.UnitZ);
            return MathFunctions.Unproject(screen, viewport, pm, vm);
        }

        /// <summary>
        /// Convert a world space coordinate into a screen space coordinate.
        /// </summary>
        /// <param name="world">The world coordinate</param>
        /// <returns>The screen coordinate</returns>
        public Coordinate WorldToScreen(Coordinate world)
        {
            var viewport = new[] { 0, 0, Width, Height };= Matrix4d.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), Width / (float)Height, 0.1f, 50000);
            var vm 
            var vm = Matrix4d.LookAt( Vector3d(Camera.Location.X, Camera.Location.Y, Camera.Location.Z),
                new Vector3d(Camera.LookAt.X, Camera.LookAt.Y, Camera.LookAt.Z),
                Vec
                Vector3d.UnitZ);
            return MathFunctions.Project(world, viewport, pm, vm);
        }

        /// <summary>
        /// Project the 2D coordinates from the screen coordinates outwards
        /// from the camera along the lookat vector, taking the frustrum
        /// into account. The resulting line will be run from the camera
        /// position along the view axis and end at the back clipping pane.
        /// </summary>
        /// <param name="x">The X coordinate on screen</param>
        /// <param name="y">The Y coordinate on screen</param>
        /// <returns>A line beginning at the camera location and tracing
        /// along the 3D projection for at least 1,000,000 units.</returns>
        public Line CastRayFromScreen(int x, int y)
        {
            var near = new Coordinate(x, Height - y, 0);
            var far = new Coordinate(x, Height - y, 1);= Matrix4d.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), Width / (float)Height, 0.1f, 50000);
            var vm 
            var vm = Matrix4d.LookAt( Vector3d(Camera.Location.X, Camera.Location.Y, Camera.Location.Z),
                new Vector3d(Camera.LookAt.X, Camera.LookAt.Y, Camera.LookAt.Z),
                Vec
                Vector3d.UnitZ);
            var viewport = new[] { 0, 0, Width, Height };
            var un = MathFunctions.Unproject(near, viewport, pm, vm);
            var uf = MathFunctions.Unproject(far, viewport, pm, vm);
            return (un == null || uf == null) ? null : new Line(un, uf);
        }
    }
}
