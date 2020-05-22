using System;
using System.Collections.Generic;
using NUnit.Framework;
using infogr_raytracer;
using OpenTK;

namespace Tests
{
    public class RectangleTest
    {
        public struct RectanglePositionTestCase
        {
            public Vector2 Position;
            public float Width;
            public float Height;
            public float Angle;
            public string Description;

            public override string ToString()
            {
                return Description;
            }
        }

        private static List<RectanglePositionTestCase> rectanglePositionTestCases =
            new List<RectanglePositionTestCase>()
            {
                new RectanglePositionTestCase()
                {
                    Position = new Vector2(0, 0),
                    Width = 4,
                    Height = 4,
                    Angle = 0,
                    Description = "Square rectangle at origin of size 4, 4"
                },
                new RectanglePositionTestCase()
                {
                    Position = new Vector2(0, 0),
                    Width = 2,
                    Height = 4,
                    Angle = 0,
                    Description = "Rectangle at origin of size 2, 4"
                },
                new RectanglePositionTestCase()
                {
                    Position = new Vector2(2, 3),
                    Width = 2,
                    Height = 4,
                    Angle = 0,
                    Description = "Rectangle at (2, 3) of size 2, 4"
                },
                new RectanglePositionTestCase()
                {
                    Position = new Vector2(2.042f, 3.036f),
                    Width = 3.2f,
                    Height = 4.2f,
                    Angle = 0,
                    Description = "Rectangle at (2.042, 3.036) of size 3.2, 4.2"
                }
            };
        
        [Test]
        [TestCaseSource("rectanglePositionTestCases")]
        public void CalculatesPosition(RectanglePositionTestCase testCase)
        {
            Rectangle rectangle = new Rectangle(testCase.Position, testCase.Width, testCase.Height);
            Assert.That(testCase.Position.X, Is.EqualTo(rectangle.Position.X).Within(0.01f));
            Assert.That(testCase.Position.Y, Is.EqualTo(rectangle.Position.Y).Within(0.01f));
        }
    }
}