using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Basics
{
    public class AnchorBasic
    {
        protected readonly ModuleDefinition Module;
        protected readonly ModuleHelper Helper;
        protected readonly BaseRefs BaseRefs;
        public AnchorBasic(ModuleDefinition module,
            ModuleHelper helper,
            BaseRefs baseRefs)
        {
            Module = module;
            Helper = helper;
            BaseRefs = baseRefs;
        }
    }
}
