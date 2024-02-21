
# ZumRailsPosts Interview Test - Avi Kedem

A brief description of what this project does and who it's for

## I've structured the solution into four layers:

#### Client
A basic Angular client interacting with the API.

#### ZumRailsPosts.API
An API layer utilizing business logic (BL) to fetch posts as per requirements

Additionally, it implements:

- Uniform API response for errors.
- Validation - Use of Model validation custom attributes  (could've implemented in many ways, starting from plain code validation in the API to validation packages etc.)
- Cross-Origin Resource Sharing (CORS) enabled for all.
- Integration with Swagger for API documentation.
- Middleware for handling exceptions.

#### ZumRailsPosts.Common
Common layer (\Helpers) for common utils and models such as MemoryCache helper and Post models


#### ZumRailsPosts.Core
Infrastructure layer, 
Includes Business Logic and Reposotory\Dal

Logic : 

Logic responsible to GetPostsAsync, by tags, sort it, and remove duplicates 
I've used few techniques here : 
- for sorting, I'm using Linq Expression for better seperation and abstraction
- for removing duplicates I'm using IEquatable ( implemanted in the Post level ), now, we can use both collection.Distinct or new HashSet<Post>(collection)

PostsRepository : 

PostsRepository is fetching posts by *a single* Tag (requirement)
It has also a cache layer, just in front of the invoking the URL

Few Notes: 
- the cache is simple, not sophisticated, it's also a singleton 
- I'm using httpClient, again, simple usages, no retries, not sophisticated error handling etc.

### Installation Instructions 

#### Client
Exceute commands : 
- npm install
- ng serve
Default URL : http://localhost:4200/

#### Client

- Default URL : http://localhost:5292/
- Example
    - GET Post with tags [tech,health] sorted by id in asc order : http://localhost:5292/api/Posts?tags=tech%2Chealth&sortBy=id&direction=asc 

## Authors

- [@Avi Kedem](https://www.github.com/AbeKdm)