README.md
Elevator Console Application
Overview
The Elevator Console Application is a console-based application designed to simulate an elevator control system in a building. It allows users to request elevators, monitor elevator statuses, and direct elevators to specific floors. This application manages multiple elevator types and includes real-time elevator status updates, error handling, and user input validation.

Features
Request Elevator: Allows users to request an elevator, specifying the current floor, direction, and load or passenger count.
Get Elevator Status: Retrieves the status of a specific elevator, including its current floor, direction, and operational status.
Real-Time Status Display: Displays the live status of all elevators with regular updates, allowing users to stop the display at any time.
Error Handling: Handles errors related to invalid inputs, unavailable elevators, and incorrect floor requests.
Project Structure
ElevatorConsoleManager: Main class responsible for handling user interactions, elevator requests, and status updates.
Dependencies:
IElevatorControlFactory: Factory interface for creating elevator control services.
IElevatorService: Interface for managing elevator operations like requesting, moving, and status retrieval.
ErrorHandler: Manages error codes and displays error messages to the user.
BuildingSettings: Configurations for the building, such as the total number of floors.
Getting Started
Configuration: Update BuildingSettings with the correct building configurations, such as the total floors in the building.
Run the Console Application: Start the application to interact with the elevator control system.
Choose an Option: The main menu offers options to request an elevator, view the status of a specific elevator, display real-time status, or exit.
Usage
Upon starting the application, the user can:

Request an Elevator by specifying the current floor, direction (Up/Down), type of elevator (Passenger, Glass, Freight), and the number of passengers or load.
Check Specific Elevator Status by entering the elevator ID to see its current state.
Display Real-Time Status to monitor all elevators, updating every second. This can be stopped by pressing 'Q'.
Error Handling
The application displays errors for invalid inputs such as non-existent floors, invalid directions, and incorrect elevator types.

Example
mathematica
Copy code
Welcome to the Elevator Control System!
1. Request Elevator
2. Get Specific Elevator Status
3. Display Real-Time Elevator Status
4. Exit
Select an option: 1
Enter current floor: 2
Enter the requested direction (Up/Down): up
Please specify the type of elevator (Passenger, Glass, Freight): Passenger
Enter number of passengers: 3
Additional Notes
Ensure IElevatorService, IElevatorControlFactory, and ErrorHandler are correctly configured with their dependencies.
The application simulates elevator movements in real-time, refreshing every second when in status display mode.
