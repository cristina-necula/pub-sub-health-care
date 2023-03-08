# pub-sub-health-care
Basic pub-sub in health care system

This solution aims to demonstrate the concepts of the pub-sub design pattern in health care domain.

The publisher is considered to be a system that is monitoring the vital signs of a patient.
The subscriber can be any system that wants to listen to updates on the patient health status.

Overall, the system:
- receives an input of data: a vital sign measurement coming from a potential sensor to a REST API endpoint; also saves it to a SQL DB
- transforms the data: verifies the new measurement against old measurements and creates one of the following alers: critical, warning, normal
- transports the data to a set of subscribers using RabbitMQ Topic Exchange
- subscribers receive the data based on the chosen subscription type and display it

![Pub-Sub in Health Care System drawio](https://user-images.githubusercontent.com/3876771/223775458-02add587-2b3c-4697-b314-6da3351c3066.png)

Solutions in this package:
- Publisher: .Net Core 7 minimal API that receives patiens and their data and transform it for sending to suscribers 
- Subscribers: Hosted service that subscribes to a certain topic and displays new messages; run with command line param routingKey="*.critical"
- Common: Common layer on top of RabbitMQ to be used by publisher and subscriber
- DataAccess.EFCore: Data access layer that handles EF 
- Domain: Domain layer that holds the entities and interfaces of the project
- PatientSimulator: background service that continuously generates a new vital sign with random values and posts them to the Publisher app in order to be transformed and send to subscribers

