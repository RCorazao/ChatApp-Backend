# Chat Application Backend
## Overview
This project is a backend implementation of a chat application, designed to handle real-time messaging, user authentication, and data storage.

## Architecture
The system consists of several key components:

**Auth Microservice:**
<br>
Handles user authentication and authorization using JWT (JSON Web Tokens) and AspNetCore.Identity with PostgreSQL.
<br><br>
**Messaging Microservice:**
<br>
Manages chat services, including sending and receiving messages, storing conversation data, and maintaining message history with MongoDB.
<br><br>
**SignalR:**
<br>
Facilitates real-time communication between clients.
<br><br>
**Proxy Layer:**
<br> 
Acts as an intermediary for handling external requests, such as fetching contact information to Auth microservice.
<br><br>

## Features
<ul>
  <li>User Authentication: Secure login and token-based authentication using JWT</li>
  <li>Real-Time Messaging: Instant message delivery using SignalR</li>
  <li>Microservices Architecture: Independent services for authentication and messaging, promoting modularity and scalability</li>
  <li>Data Persistence: Separate storage solutions for authentication and messaging data</li>
</ul>

## Technologies Used
<ul>
  <li>.NET</li>
  <li>SignalR</li>
  <li>JSON Web Tokens</li>
  <li>PostgreSQL</li>
  <li>MongoDB</li>
</ul>
