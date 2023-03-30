Feature: Room Reservation
when a room is free in selected date times, then a user can be reserved the room

@mytag
Scenario:2 reserved room in free times
	Given I selected a speciefic room 
	And choosed the start datetime and end datetime in free times for reservation
	When set the room with start datetime and end datetime
	Then room should be reserved in specific datetimes

Scenario:1 reserved room in busy times
	Given I selected a speciefic room 
	And choosed the start datetime and end datetime in busy times for reservation
	When set the room with start datetime and end datetime
	Then room could not be reserved in specific datetimes and retruned error



