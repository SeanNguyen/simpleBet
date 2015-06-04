using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using SimpleBet.Models;

namespace SimpleBet.Migrations
{
    [ContextType(typeof(SimpleBetContext))]
    partial class init
    {
        public override string Id
        {
            get { return "20150604080939_init"; }
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
                
                builder.Entity("SimpleBet.Models.Bet", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("Question")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int?>("UserId")
                            .Annotation("OriginalValueIndex", 2)
                            .Annotation("ShadowIndex", 0);
                        b.Key("Id");
                    });
                
                builder.Entity("SimpleBet.Models.Option", b =>
                    {
                        b.Property<int?>("BetId")
                            .Annotation("OriginalValueIndex", 0)
                            .Annotation("ShadowIndex", 0);
                        b.Property<string>("Content")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 2)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Key("Id");
                    });
                
                builder.Entity("SimpleBet.Models.User", b =>
                    {
                        b.Property<int?>("FacebookId")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 1)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 2);
                        b.Key("Id");
                    });
                
                builder.Entity("SimpleBet.Models.ValueModel", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Key("Id");
                    });
                
                builder.Entity("SimpleBet.Models.Bet", b =>
                    {
                        b.ForeignKey("SimpleBet.Models.User", "UserId");
                    });
                
                builder.Entity("SimpleBet.Models.Option", b =>
                    {
                        b.ForeignKey("SimpleBet.Models.Bet", "BetId");
                    });
                
                return builder.Model;
            }
        }
    }
}
