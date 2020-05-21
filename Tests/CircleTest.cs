using System;
using System.Collections.Generic;
using NUnit.Framework;
using infogr_raytracer;
using OpenTK;

namespace Tests
{
    public class CircleTest
    {
        public struct CircleTestCase
        {
            public Ray Ray;
            public Circle Circle;
            public bool ExpectedResult;
            public string Description;

            public override string ToString()
            {
                return Description;
            }
        }

        private static List<CircleTestCase> testCases = new List<CircleTestCase>()
        {
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray()
                {
                    Origin = new Vector2(4, 0),
                    Direction = new Vector2(-1, 0),
                    Magnitude = 5f, 
                },
                ExpectedResult = true,
                Description = "Intersection near origin"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 2), Direction = new Vector2(1, 0), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(2, 4), Direction = new Vector2(0, -1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from top"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 2), Direction = new Vector2(-1, 0), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from right"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, -2), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from bottom"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(-4, 4), Direction = new Vector2(1, -1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from top left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(-4, -4), Direction = new Vector2(1, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from bottom left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(-1, -1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from top right"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, -4), Direction = new Vector2(-1, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Orthogonal from bottom right"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 3), Direction = new Vector2(1, 0), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Touch the circle at the bottom"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 10), Direction = new Vector2(1, 0), Magnitude = 50f },
                ExpectedResult = false,
                Description = "Ray that flies above the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, -10), Direction = new Vector2(1, 0), Magnitude = 50f },
                ExpectedResult = false,
                Description = "Ray that flies beneath the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 0), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = false,
                Description = "Ray that flies to the left of the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(6, 0), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = false,
                Description = "Ray that flies to the right of the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Ray from the inside the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 0.1f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Ray from the inside of a smaller the circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 0.001f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(0, 1), Magnitude = 50f },
                ExpectedResult = true,
                Description = "Ray from the inside of a tiny circle"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 0.001f},
                Ray = new Ray() { Origin = new Vector2(0, 0), Direction = new Vector2(1, 1), Magnitude = 1f },
                ExpectedResult = false,
                Description = "Ray end before reaching intersection"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(4, 4), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 0), Direction = new Vector2(1, 1), Magnitude = 1f },
                ExpectedResult = false,
                Description = "Ray end before reaching intersection"
            }
        };
        
        [Test]
        [TestCaseSource("testCases")]
        public void TestIntersect(CircleTestCase testCase)
        {
            Console.WriteLine(testCase.Description);
            Assert.AreEqual(testCase.Circle.Intersects(testCase.Ray), testCase.ExpectedResult, testCase.Description);
        }
    }
}