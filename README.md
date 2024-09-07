### Introduction to GraphQL

- **Launched in 2015 by Facebook**:
  - Facebook introduced GraphQL in 2015. Initially, they faced challenges where the startup of the app initialized a bunch of requests to get data related to the user (such as posts, events, friend suggestions, etc.), consuming resources. Each request brought too much data, much of which was not required. They invented GraphQL to handle these issues, making it easier for the client to handle one request to get the needed user data in one go.
  
- **Client-Specified Data Retrieval**:
  - The client or user specifies exactly which properties to retrieve, preventing the request of all properties in a single query, which makes the data retrieval process more efficient and targeted.

- **GraphiQL Tool**:
  - GraphiQL is a tool used to explore GraphQL backend queries and responses automatically without writing any code.

  [Reference](https://codetraveler.io/dotnetgraphql/)

### GraphQL vs REST

- **REST**:
  - An architectural concept for network-based software.
  - Has no official set of tools or specification.
  - Does not care if you use HTTP, AMQP, etc.
  - Designed to decouple an API from the client.
  - Focuses on making APIs last for decades, instead of optimizing for performance.

- **GraphQL**:
  - A query language, specification, and collection of tools.
  - Designed to operate over a single endpoint via HTTP.
  - Optimizes for performance and flexibility.
  - Invents its own conventions, unlike REST, which utilizes the uniform interface of the protocols it exists in.

- **REST Utilizes Protocol Features**:
  - When utilizing HTTP, REST can leverage HTTP content-types, caching, status codes, etc.
  - GraphQL, on the other hand, invents its own conventions.

- **When to Use GraphQL**:
  - If your API is not using hypermedia controls, then GraphQL could be a more relevant approach, because you weren't really using REST anyway but we still can see the simplicity and familiarity of using consuming REST apis.

### GraphQL Operations

- **Query**:
  - Used to query over data.

- **Mutation**:
  - Used to create, update, or delete data.

- **Subscription**:
  - Used to subscribe to events based on web sockets in C#.
 
## N+1 Problem

When querying relational data, consider the following example:
**Course → Instructor**: Each course has an instructor. Now, let's say we're retrieving the courses. There are two options in our repository:

1. Always include the instructor when retrieving the courses. **(Not good)**  
   This can negatively impact performance since the client might not need the instructor data in the query schema.
   
2. Retrieve the instructor only if requested in the query schema. **(Good)**  
   However, even with this approach, there’s still another issue. Let’s say we have 7 courses in the database. In this case, we would need to make N+1 hits to the database: N times for the instructor of each course and +1 for retrieving the courses themselves. This is known as the **N+1 problem**.

## Data Loader

A **DataLoader** is an approach used to reduce N trips to the database to just one. It allows us to fetch the required data in batches. In the example of instructors and courses, we can create an `InstructorDataLoader` that inherits from `BatchDataLoader`. This class maintains a dictionary of keys and values. In our case, the key would be of type `Guid` (the instructor ID), and the value would be of type `Instructor`.

The `BatchDataLoader` includes a `LoadAsync` method, which is called for each instructor with each course. At the end, it triggers the repository to fetch all instructors in a single trip to the database and map the instructor to each course by ID.

## Pagination

There are two types of pagination:

1. **Cursor Pagination**
2. **Offset Pagination**

**Cursor pagination** is recommended for several reasons:

### Performance
When using offset pagination, we use `Skip` and `Take`. As the offset increases, a large number of rows must be scanned before retrieving the required data, which slows down the query. With cursor pagination, we can use a specific cursor to identify the rows, making it more efficient and eliminating the need to scan all skipped rows.

### Scalability
As the offset increases, queries become slower, making cursor-based pagination a more scalable solution.

## Authentication and Authorization

- You can use the following repo to generate jwtToken --> [JwtGenerator](https://github.com/khaledgomaa/JwtGenerator).
