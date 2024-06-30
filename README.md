# Break Time Task

This program find the busiest break period from the provided bus driver break times.

## Features

- Processes break times from a text file at startup
- Accepts break times in the format "HH:mmHH:mm" (example "14:0014:30")
- Allows additional break times to be entered via command line during runtime
- Calculates and displays the busiest break period after each new entry

## How to Use

### Running the Program

To run the program, use the following command in your terminal:

```console
dotnet run
```

To run the program with the text file with the break times, use the following command in your terminal:

```console
dotnet run filename <filepath>
```

Break times entries should be on separate lines in a format"HH:mmHH:mm"
