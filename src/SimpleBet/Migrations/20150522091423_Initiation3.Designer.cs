using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using SimpleBet.Models;

namespace SimpleBet.Migrations
{
    [ContextType(typeof(SimpleBetContext))]
    partial class Initiation3
    {
        public override string Id
        {
            get { return "20150522091423_Initiation3"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta4-12943"; }
        }
        
        public override IModel Target
        {
            get
            {
                var builder = new BasicModelBuilder()
                    .Annotation("SqlServer:ValueGeneration", "Sequence");
                
                builder.Entity("SimpleBet.Models.ValueModel", b =>
                    {
                        b.Property<int>("Age")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 1)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("Value")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                    });
                
                return builder.Model;
            }
        }
    }
}
