@echo off
eventcreate /SO Scheduler.Web              /L APPLICATION /T INFORMATION /ID 1 /D "Scheduler.Web event source created."
eventcreate /SO Scheduler.SchedulerService /L APPLICATION /T INFORMATION /ID 1 /D "Scheduler.SchedulerService event source created."
