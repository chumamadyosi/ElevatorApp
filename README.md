**Class Overview**

The ElevatorConsoleManager class is the main console application responsible for handling user interactions to manage elevator operations. It leverages various dependencies injected through the constructor for elevator management, including factory services, building settings, and error handling.
Core Methods and Workflow
**1.	StartAsync()**

This is the main loop, displaying menu options for the user:

    o	Option 1: Request an elevator.
    
    o	Option 2: Retrieve a specific elevator’s status.
    
    o	Option 3: Display real-time status of all elevators.
    
    o	Option 4: Exit the application.

Based on the user’s choice, the method invokes specific functionalities. If an error occurs during any operation, it’s managed via the ErrorHandler.

**3.	RequestElevator()**

Manages the process of requesting an elevator, which includes:
    o	Collecting the user's request details (e.g., floor, direction, elevator type).
    
    o	Finding the nearest available elevator.
    
    o	Loading passengers or freight.
    
    o	Moving the elevator to the requested floor.
    
    o	Prompting the user for a destination floor once the elevator arrives.

Errors, such as invalid floor requests or capacity issues, are returned as error codes.

**4.	GetSpecificElevatorStatus()**

Allows the user to retrieve and display the status of a specific elevator by entering its ID. Errors like an invalid elevator ID are managed here.

**5.	DisplayRealTimeElevatorStatus()**

Continuously displays the real-time status of all elevators in the system until the user decides to stop. The status updates every second.

**6.	Helper Methods**

    o	GetLiftRequest(): Collects details of the elevator request, such as floor number, direction, type, and passenger count/load. It returns an ElevatorRequest object or an error code for invalid inputs.
    o	WaitForElevatorArrival(): Awaits the elevator’s arrival at the specified floor by polling every 500 milliseconds.
Dependencies
    •	IElevatorControlFactory: Creates control services based on the elevator type.
    •	IElevatorService: Manages core elevator operations, such as movement and status.
    •	ErrorHandler: Handles errors in elevator operations.
    •	BuildingSettings: Configures building-specific settings (e.g., total floors).
    
**Usage and Error Handling**

      •	The code prompts the user at each step, validating inputs and handling errors for invalid requests (e.g., requesting an out-of-range floor).
      •	If errors occur during elevator requests or real-time updates, they are processed by the ErrorHandler.
  
 ** RequestElevator Method Documentation**


The RequestElevator method is an asynchronous operation that coordinates the entire elevator request and movement process. Its design includes input validation, locating the nearest elevator, loading occupants, and ensuring safe elevator travel to the specified floor. Each step has error checks that return relevant error codes if issues are detected, promoting reliable and smooth elevator operations. Here is a detailed breakdown:
Steps in the RequestElevator Method:

**1.	Collecting Elevator Request Details:**

      o	The method initiates by calling GetLiftRequest to collect user details, such as the requested floor, direction, elevator type (e.g., passenger), and load capacity.
      o	If any input is invalid, GetLiftRequest returns an error code, which the method checks for and returns immediately if present.
      
**2.	Creating the Elevator Control Service:**

    o	The method initializes an elevatorFactory using the CreateElevatorControlService function. This factory configures specific operations according to the requested elevator type, managing loading and movement as per user specifications.
    
**3.	Finding the Nearest Elevator:**
	
      o	Using GetNearestElevator, the method identifies the closest available elevator that aligns with the requested floor and direction.
      o	If no suitable elevator is found, an error code is returned to halt further processing.
      
**4.	Loading Occupants or Freight into the Elevator:**

      o	After locating an elevator, the method uses the elevatorFactory to load passengers or freight based on the user-defined capacity. This ensures the elevator is prepared before proceeding.
      o	If the capacity exceeds limits, an error code is returned, stopping the request.
      
**5.	Requesting the Elevator to Arrive at the User’s Floor:**

      o	The method sends a command via RequestElevatorToFloor to move the elevator to the user’s current floor.
      o	If the elevator cannot reach the floor, an error code is returned.
      
**6.	Waiting for the Elevator’s Arrival:**

      o	Once the elevator is en route, the method waits until it arrives at the user’s floor, ensuring the user is not prompted until the elevator is ready.
      o	Upon arrival, a confirmation message notifies the user that boarding can commence.
      
**7.	Prompting the User for Destination Floor:**

      o	The method prompts the user for their destination floor and verifies that it falls within the building’s range (e.g., between floor 1 and the maximum floor).
      o	If the destination is invalid, an error code is returned.
  	
**8.	Adding Occupants After Elevator Arrival:**

      o	After arrival at the user’s floor, the method verifies load capacity one final time as a precaution to ensure limits are respected.
      o	If limits are exceeded, an error code indicates this issue.
      
**9.	Moving the Elevator to the Destination Floor:**

      o	The method directs the elevator to the specified destination floor using MoveElevatorToDestinationFloor.
      o	A message is displayed, informing the user that the elevator is en route.
      
**10.	Returning the Result:**

•	If all steps complete successfully, the method returns null, signaling success. Otherwise, any encountered error code is returned immediately, halting further actions.

