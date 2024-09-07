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
  - If your API is not using hypermedia controls, then GraphQL could be a more relevant approach, because you weren't really using REST anyway.

### GraphQL Operations

- **Query**:
  - Used to query over data.

- **Mutation**:
  - Used to create, update, or delete data.

- **Subscription**:
  - Used to subscribe to events based on web sockets in C#.

### Authentication

- You can use the following repo to generate jwtToken --> [JwtGenerator](https://github.com/khaledgomaa/JwtGenerator).
