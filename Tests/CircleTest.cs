using System;
using System.Collections.Generic;
using NUnit.Framework;
using infogr_raytracer;
using OpenTK;

namespace Tests
{
    public class CircleTest
    {
        private static Circle circle = new Circle()
        {
            Position = new Vector2(2f,2f),
            Radius = 1f,
        };
        
        private static List<(Ray, bool, string)> cases = new List<(Ray, bool, string)>()
        {
            (new Ray() {Origin = new Vector2(0, 2), Direction = new Vector2(1, 0)}, true, "A"),
            (new Ray() {Origin = new Vector2(0, 2), Direction = new Vector2(1, 0)}, true, "B")
        };
        
        [Test]
        [TestCaseSource("cases")]
        public void TestIntersect((Ray, bool, string) testCase)
        {
            Assert.AreEqual(testCase.Item2, circle.Intersects(testCase.Item1), testCase.Item3);
        }
    }
}