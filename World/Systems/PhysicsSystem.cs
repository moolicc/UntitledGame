using System;
using System.Collections.Generic;
using System.Text;

namespace UntitledGame.World
{
    class PhysicsSystem : EntityManagement.SystemBase
    {
        public PhysicsSystem()
            : base(typeof(EntityComponents.BoundsComponent), typeof(EntityComponents.PhysicsComponent))
        {
        }

        public void Step()
        {
            // This should simulate physics for the current frame.
        }
    }
}
