using Microsoft.AspNet.Mvc;
using SimpleBet.Controllers;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBet.Test.Api
{
    public class WinningItemControllerTest
    {
        private WinningItem winningItemNoId
        {
            get
            {
                return new WinningItem()
                                    {
                                        Type = WINNING_ITEM_TYPE.NONMONETARY,
                                        Title = "Punch",
                                        Description = "I will punch you",
                                        CreatorId = 1
                                    };
            }
        }

        private WinningItem winningItem
        {
            get
            {
                return new WinningItem()
                                    {
                                        Id = 2,
                                        Type = WINNING_ITEM_TYPE.MONETARY,
                                        Title = "Wine",
                                        Description = "I will buy you this",
                                        CreatorId = 2
                                    };
            }
        }

        private IDataService dataService = new DataService();

        [Fact]
        public void Get()
        {
            WinningItem actual = this.dataService.GetWinningItem(2);
            WinningItem expected = this.winningItem;

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.CreatorId, actual.CreatorId);
        }

        [Fact]
        public void GetInvalid()
        {
            WinningItem actual = this.dataService.GetWinningItem(-100);
            Assert.Null(actual);
        }

        [Fact]
        public void Add()
        {
            WinningItem actual = this.dataService.AddWinningItem(this.winningItemNoId);
            WinningItem expected = this.winningItemNoId;

            Assert.NotNull(actual.Id);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.CreatorId, actual.CreatorId);
        }

        [Fact]
        public void Update()
        {
            WinningItem expected = this.winningItem;
            expected.Title = "Changed Title";
            expected.Description = "Changed more";
            expected.Type = WINNING_ITEM_TYPE.NONMONETARY;
            expected.CreatorId = 1;
            WinningItem actual = this.dataService.UpdateWinningItem(expected);
            
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.CreatorId, actual.CreatorId);
        }
    }
}
