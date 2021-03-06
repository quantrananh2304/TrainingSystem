//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Topic
    {
        public int TopicID { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string TopicName { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string TopicDescription { get; set; }
        public int CourseID { get; set; }
        public int TrainerID { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}
