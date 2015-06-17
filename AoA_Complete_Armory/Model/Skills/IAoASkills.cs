using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model.Skills
{
    public interface IAoASkills
    {
        SkillType Type { get; }
    }

    public enum SkillType 
    {
        Capture_POW 
    }
}
