## Main
![Build - Test - Main](https://github.com/IngSoft-DA2/266824_329180_287195/actions/workflows/build-test.yml/badge.svg?branch=main&event=push)
![Clean Code - Main](https://github.com/IngSoft-DA2/266824_329180_287195/actions/workflows/code-analysis.yml/badge.svg?branch=main&event=push)

## Develop
![Build - Test - Develop](https://github.com/IngSoft-DA2/266824_329180_287195/actions/workflows/build-test.yml/badge.svg?branch=develop&event=push)
![Clean Code - Develop](https://github.com/IngSoft-DA2/266824_329180_287195/actions/workflows/code-analysis.yml/badge.svg?branch=develop&event=push)


# 🎢 VirtualPark

VirtualPark is a full-stack academic project developed for the **Design of Applications II** course.  
The system simulates the management of an amusement park, providing tools for managing attractions, events, tickets, visitors, rewards, and rankings.

The project was built following modern software engineering practices, including Clean Architecture, SOLID principles, and Test-Driven Development (TDD).

---

## 📌 Features

### 🎟 Ticket Management
- Ticket creation and validation
- Entry validation via NFC and QR
- Capacity and availability checks

### 🎡 Attraction Management
- CRUD operations for attractions
- Age, capacity, and availability validation
- Logical deletion with historical preservation

### 🎭 Event Management
- Event creation and management
- Attraction assignment to events
- Event capacity and date validation

### 🧑 Visitor Management
- Visitor profiles
- Visit registrations tracking
- Ranking and scoring system

### 🏆 Rewards System
- Reward creation and redemption
- Points validation and stock management
- Redemption history tracking

### 🚨 Incident Management
- Incident creation and tracking
- Type-based categorization
- Activation and deactivation of incidents

---

## 🧱 Architecture

The project follows **Clean Architecture** and **Domain Driven Design concepts**, separating responsibilities across layers:

- Controllers
- Services
- Domain Entities
- Repositories
- DTOs (Args & Responses)

---

## 🧪 Testing

The project follows **Test-Driven Development (TDD)**.

Technologies used:
- MSTest
- FluentAssertions
- Moq

Tests cover:
- Services
- Entities validation
- Business rules
- Controller behavior

---

## ⚙️ Technologies Used

### Backend
- C# .NET 8
- ASP.NET Web API
- Entity Framework Core
- SQL Server

### Frontend
- Angular
- TypeScript
- Reactive Forms

### DevOps & Quality
- GitHub Actions
- Code Analysis
- CI/CD Pipelines

---

## 📊 Development Practices

- SOLID Principles
- GRASP Patterns
- Repository Pattern
- Strategy Pattern
- Clean Code
- GitFlow workflow
- Unit Testing coverage

---
