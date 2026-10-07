# Social Media Platform

## Project Overview
A comprehensive social media platform consisting of three main components: Backend (Spring Boot), Frontend (Vue.js), and Desktop Admin Application (Windows Forms). The platform allows users to create posts, comment, and interact while providing administrators with powerful moderation tools.

## Architecture

### Backend (Spring Boot)
- RESTful API built with Spring Boot
- JWT-based authentication
- Role-based access control (User/Admin)
- Email notifications for post status changes
- PostgreSQL database
- Key features:
  - User management
  - Post creation and moderation
  - Comment system
  - Admin operations

### Frontend (Vue.js + Vuetify)
- Modern SPA built with Vue 3 and Vuetify
- Responsive design
- Features:
  - User authentication
  - Post creation and viewing
  - Comment management
  - Real-time status updates
  - Admin panel for post moderation

### Desktop Admin Application (.NET Windows Forms)
- Native Windows application for administrators
- Features:
  - Secure admin authentication
  - Post moderation (Approve/Reject)
  - User management
  - Comment monitoring
  - Batch operations

## Key Features

### User Features
- Register and login
- Create posts (pending admin approval)
- View approved posts
- Add comments to posts
- Edit/delete own comments
- View post status

### Admin Features
- Dedicated admin panel (web and desktop)
- Post moderation (approve/reject)
- User management (ban users)
- Comment moderation
- System monitoring

## Technical Details

### Backend
- Spring Boot 3.x
- Spring Security with JWT
- JPA/Hibernate
- PostgreSQL
- Email integration
- RESTful API endpoints

### Frontend
- Vue 3
- Vuetify 3
- Pinia for state management
- Vue Router
- Axios for API communication
- Responsive design

### Desktop Admin App
- .NET 8.0 Windows Forms
- Modern UI design
- Async operations
- Secure token management
- Grid-based data management

## API Endpoints

### Authentication
- POST /auth/register - User registration
- POST /auth/login - User login
- POST /auth/register-admin - Admin registration

### Posts
- GET /post - Get all posts
- GET /post/published - Get published posts
- POST /post - Create new post
- PUT /post/{id} - Update post
- DELETE /post/{id} - Delete post

### Admin Operations
- GET /admin/users - Get all users
- PUT /admin/posts/{id}/approve - Approve post
- PUT /admin/posts/{id}/reject - Reject post
- DELETE /admin/users/{id} - Ban user

## Security
- JWT-based authentication
- Role-based authorization
- Secure password hashing
- CORS configuration
- Token validation

## Installation & Setup

### Backend
1. Clone repository
2. Configure database settings
3. Run Spring Boot application
4. Access API at http://localhost:8083

### Frontend
1. Install dependencies: `npm install`
2. Configure API endpoint
3. Run development server: `npm run dev`
4. Access at http://localhost:5173

### Desktop Admin App
1. Open solution in Visual Studio
2. Configure API endpoint in `ApiConfig.cs`
3. Build and run the application

## Development Workflow
1. Users create posts (status: PENDING)
2. Admins review posts via web or desktop app
3. Posts are approved/rejected
4. Users receive email notifications
5. Approved posts appear in the feed

## Contributors
- [Dobrovolschi Alexandru] - Full-stack development
