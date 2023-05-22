# Assessment Overview
The provided assessment in its simplest form seems to be a variation of the "Subset Sum Problem" where the objective is to find the subset of elements in a given sequence that sums up to a certain value (0 in this case) or exceeds a certain value.

## Visualizing the Solution
This specific implementation of the solution uses a [binary tree based association (inclusion/exclusion) mechanism](https://www.brainkart.com/article/Subset--Sum-problem_7979/) to traverse all possible subsets to identify the correct one which satisfies the given condition (sum is 0, sum exceeds by the least amount, etc.). Since the current implementation depends upon this binary tree (2 possibilities for each element in the form of inclusion and exclusion) consisting upto n number of levels, the time complexity is O(2^N). See figure below,

![binary tree figure](https://img.brainkart.com/imagebk8/jgh1fjT.jpg)

## Further Considerations
It may be possible to [reduce the number of operations by maintaining a remainder in each node](https://youtu.be/kyLxTdsT8ws) which can then be used to specify additional exit criteria for the recursive calculation. This was not implemented due to time constraints.

## Codebase Overview
The code is pretty much self explanatory and includes additional comments in critical areas. To build and test;
* Clone this repository
* Restore dependencies with `dotnet restore`
* Build with `dotnet build`
* Run tests with `dotnet test`
