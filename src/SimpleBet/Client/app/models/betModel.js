'use-strict';

var WAGER_TYPE = {Monetary: 'Monetary', NonMonetary: 'NonMoneytary'};

function BetModel() {
	this.question;
	this.team = [{sizeMax: 10, sizeMin: 0}, {sizeMax: 10, sizemin: 0}];
	this.options = [];
	this.wagerType = 

	//public methods

	this.addOption = function(inputOption) {
		this.options.push(inputOption);
	}

	this.removeOption = function(index) {
		this.options.splice(index);
	}

	this.getMaxTeamSize = function(team) {
		return this.team[team].sizeMax;
	}

	this.setMaxTeamSize = function(team, size) {
		return this.team[team].sizeMax = size;
	}

	this.setWagerType = function (type) {
		if(type === WAGER_TYPE.Monetary || type === WAGER_TYPE.NonMoneytary) {
			this.wagerType = type;
		}
	}
}