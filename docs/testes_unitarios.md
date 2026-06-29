# Casos de Teste Unitários - MediConnect WebAPI

## Visão Geral

Este documento descreve os casos de teste unitários para cada módulo (service) da camada `WebApi.Services`.

---

## 1. AddressService

### `GetAllAsync()`
- CT-001: Deve retornar todos os endereços ordenados por Id decrescente
- CT-002: Deve retornar lista vazia quando não houver endereços

### `GetByIdAsync(int id)`
- CT-003: Deve retornar o endereço quando o Id existe
- CT-004: Deve retornar null quando o Id não existe

### `AddAsync(Address address)`
- CT-005: Deve adicionar um endereço válido e retorná-lo com Id gerado
- CT-006: Deve lançar exceção quando o endereço for inválido

### `UpdateAsync(Address address)`
- CT-007: Deve atualizar o endereço e definir UpdatedAt
- CT-008: Deve lançar exceção quando o endereço não existir

### `DeleteAsync(Address address)`
- CT-009: Deve remover o endereço com sucesso
- CT-010: Deve lançar exceção quando o endereço não existir

---

## 2. AdminAuthService

### `ValidateUserAsync(string email, string password)`
- CT-011: Deve retornar o administrador quando email e senha forem válidos
- CT-012: Deve retornar null quando a senha for inválida
- CT-013: Deve retornar null quando o email não existir

### `CreateToken(Administrator administrador)`
- CT-014: Deve gerar um token JWT válido para o administrador
- CT-015: Deve lançar exceção quando a chave secreta estiver vazia
- CT-016: O token gerado deve expirar em 1 hora

### `CreateRefreshToken(Administrator administrator)`
- CT-017: Deve gerar um refresh token JWT válido
- CT-018: O refresh token deve expirar em 7 dias

### `ValidateRefreshToken(string refreshToken)`
- CT-019: Deve retornar ClaimsPrincipal para um refresh token válido
- CT-020: Deve retornar null para um refresh token inválido/expirado

### `GetUserAsync(int id)`
- CT-021: Deve retornar o administrador quando o Id existe
- CT-022: Deve retornar null quando o Id não existe

### `CreateUserAsync(Administrator administrator)`
- CT-023: Deve criar um novo administrador com sucesso

### `FindUserByEmailAsync(string email)`
- CT-024: Deve retornar true quando o email já existe
- CT-025: Deve retornar false quando o email não existe

---

## 3. AdministratorService

### `GetAllAsync()`
- CT-026: Deve retornar todos os administradores exceto o seed admin (administrador@administrador.com)
- CT-027: Deve retornar lista vazia quando não houver administradores

### `GetByIdAsync(int id)`
- CT-028: Deve retornar o administrador quando o Id existe
- CT-029: Deve retornar null quando o Id não existe

### `AddAsync(Administrator administrator)`
- CT-030: Deve adicionar um administrador com senha hasheada
- CT-031: Deve lançar exceção quando ocorrer erro no banco

### `UpdateAsync(Administrator administrator)`
- CT-032: Deve atualizar o administrador e definir UpdatedAt

### `DeleteAsync(Administrator administrator)`
- CT-033: Deve remover o administrador com sucesso

### `GetByEmailAsync(string email, int? id)`
- CT-034: Deve retornar null quando o email não está em uso
- CT-035: Deve retornar o administrador quando o email já existe
- CT-036: Deve ignorar o próprio Id na verificação (para edição)

---

## 4. AppointmentService

### `GetAllAsync()`
- CT-037: Deve retornar todos os agendamentos com includes (Doctor, MedicalCenter, User, Rating)
- CT-038: Deve retornar lista vazia quando não houver agendamentos

### `GetByIdAsync(int id)`
- CT-039: Deve retornar o agendamento com includes quando o Id existe
- CT-040: Deve retornar null quando o Id não existe

### `AddAsync(Appointment appointment)`
- CT-041: Deve adicionar um agendamento com protocolo gerado automaticamente
- CT-042: Deve lançar exceção quando ocorrer erro no banco

### `UpdateAsync(Appointment appointment)`
- CT-043: Deve atualizar o agendamento e definir UpdatedAt

### `DeleteAsync(Appointment appointment)`
- CT-044: Deve remover o agendamento com sucesso

---

## 5. AppointmentRatingService

### `GetAllAsync()`
- CT-045: Deve retornar todas as avaliações com include de Appointment
- CT-046: Deve retornar lista vazia quando não houver avaliações

### `GetByIdAsync(int id)`
- CT-047: Deve retornar a avaliação com include quando o Id existe
- CT-048: Deve retornar null quando o Id não existe

### `AddAsync(AppointmentRating appointmentRating)`
- CT-049: Deve adicionar uma avaliação com sucesso

### `UpdateAsync(AppointmentRating appointmentRating)`
- CT-050: Deve atualizar a avaliação e definir UpdatedAt

### `DeleteAsync(AppointmentRating appointmentRating)`
- CT-051: Deve remover a avaliação com sucesso

---

## 6. DoctorService

### `GetAllAsync()`
- CT-052: Deve retornar todos os médicos com includes (Specialization, DoctorMedicalCenters)
- CT-053: Deve retornar lista vazia quando não houver médicos

### `GetByIdAsync(int id)`
- CT-054: Deve retornar o médico com include de Specialization
- CT-055: Deve retornar null quando o Id não existe

### `AddAsync(Doctor doctor)`
- CT-056: Deve adicionar um médico com sucesso

### `UpdateAsync(Doctor doctor)`
- CT-057: Deve atualizar o médico e definir UpdatedAt

### `DeleteAsync(Doctor doctor)`
- CT-058: Deve remover o médico com sucesso

---

## 7. HealthPlanService

### `GetAllAsync()`
- CT-059: Deve retornar todos os planos de saúde com include de MedicalAgreements
- CT-060: Deve retornar lista vazia quando não houver planos

### `GetByIdAsync(int id)`
- CT-061: Deve retornar o plano com include quando o Id existe
- CT-062: Deve retornar null quando o Id não existe

### `AddAsync(HealthPlan healthplan)`
- CT-063: Deve adicionar um plano de saúde com sucesso

### `UpdateAsync(HealthPlan healthplan)`
- CT-064: Deve atualizar o plano e definir UpdatedAt

### `DeleteAsync(HealthPlan healthplan)`
- CT-065: Deve remover o plano com sucesso

---

## 8. MedicalAgreementService

### `GetAllAsync()`
- CT-066: Deve retornar todos os convênios ordenados por Id decrescente
- CT-067: Deve retornar lista vazia quando não houver convênios

### `GetByIdAsync(int id)`
- CT-068: Deve retornar o convênio quando o Id existe
- CT-069: Deve retornar null quando o Id não existe

### `AddAsync(MedicalAgreement medicalagreement)`
- CT-070: Deve adicionar um convênio com sucesso

### `UpdateAsync(MedicalAgreement medicalagreement)`
- CT-071: Deve atualizar o convênio e definir UpdatedAt

### `DeleteAsync(MedicalAgreement medicalagreement)`
- CT-072: Deve remover o convênio com sucesso

---

## 9. MedicalCenterService

### `GetAllAsync()`
- CT-073: Deve retornar todas as unidades com includes (DoctorMedicalCenters, Address)
- CT-074: Deve retornar lista vazia quando não houver unidades

### `GetByIdAsync(int id)`
- CT-075: Deve retornar a unidade com includes quando o Id existe
- CT-076: Deve retornar null quando o Id não existe

### `AddAsync(MedicalCenter medicalCenter)`
- CT-077: Deve adicionar uma unidade com sucesso

### `UpdateAsync(MedicalCenter medicalCenter)`
- CT-078: Deve atualizar a unidade e definir UpdatedAt

### `DeleteAsync(MedicalCenter medicalCenter)`
- CT-079: Deve remover a unidade com sucesso

### `GetByEmailAsync(string email, int? id)`
- CT-080: Deve retornar null quando o email não está em uso
- CT-081: Deve retornar a unidade quando o email já existe

---

## 10. MedicalExamService

### `GetAllAsync()`
- CT-082: Deve retornar todos os exames ordenados por Id decrescente
- CT-083: Deve retornar lista vazia quando não houver exames

### `GetByIdAsync(int id)`
- CT-084: Deve retornar o exame quando o Id existe
- CT-085: Deve retornar null quando o Id não existe

### `AddAsync(MedicalExam medicalExam)`
- CT-086: Deve adicionar um exame com sucesso

### `UpdateAsync(MedicalExam medicalExam)`
- CT-087: Deve atualizar o exame e definir UpdatedAt

### `DeleteAsync(MedicalExam medicalExam)`
- CT-088: Deve remover o exame com sucesso

---

## 11. SpecializationService

### `GetAllAsync()`
- CT-089: Deve retornar todas as especializações ordenadas por Id decrescente
- CT-090: Deve retornar lista vazia quando não houver especializações

### `GetByIdAsync(int id)`
- CT-091: Deve retornar a especialização quando o Id existe
- CT-092: Deve retornar null quando o Id não existe

### `AddAsync(Specialization Specialization)`
- CT-093: Deve adicionar uma especialização com sucesso

### `UpdateAsync(Specialization Specialization)`
- CT-094: Deve atualizar a especialização e definir UpdatedAt

### `DeleteAsync(Specialization Specialization)`
- CT-095: Deve remover a especialização com sucesso

---

## 12. UserAuthService

### `ValidateUserAsync(string email, string password)`
- CT-096: Deve retornar o usuário quando email e senha forem válidos
- CT-097: Deve retornar null quando a senha for inválida
- CT-098: Deve retornar null quando o email não existir

### `CreateToken(User user)`
- CT-099: Deve gerar um token JWT válido para o usuário (usando UserSecretKey)
- CT-100: O token gerado deve expirar em 1 hora

### `CreateRefreshToken(User user)`
- CT-101: Deve gerar um refresh token JWT válido
- CT-102: O refresh token deve expirar em 7 dias

### `ValidateRefreshToken(string refreshToken)`
- CT-103: Deve retornar ClaimsPrincipal para um refresh token válido
- CT-104: Deve retornar null para um refresh token inválido/expirado

### `GetUserAsync(int id)`
- CT-105: Deve retornar o usuário com include de address quando o Id existe
- CT-106: Deve retornar null quando o Id não existe

### `CreateUserAsync(User user)`
- CT-107: Deve criar um usuário com senha hasheada

### `FindUserByEmailAsync(string email)`
- CT-108: Deve retornar true quando o email já existe
- CT-109: Deve retornar false quando o email não existe

### `CreateAddressAndBindUser(Address address, int id)`
- CT-110: Deve criar um endereço e associá-lo ao usuário
- CT-111: Deve lançar exceção quando o usuário não existe

---

## 13. UserService

### `GetAllAsync()`
- CT-112: Deve retornar todos os usuários ordenados por Id decrescente
- CT-113: Deve retornar lista vazia quando não houver usuários

### `GetByIdAsync(int id)`
- CT-114: Deve retornar o usuário quando o Id existe
- CT-115: Deve retornar null quando o Id não existe

### `AddAsync(User user)`
- CT-116: Deve adicionar um usuário com senha hasheada
- CT-117: Deve lançar exceção quando ocorrer erro no banco

### `UpdateAsync(User user)`
- CT-118: Deve atualizar o usuário e definir UpdatedAt

### `DeleteAsync(User user)`
- CT-119: Deve remover o usuário com sucesso

### `GetByEmailAsync(string email, int? id)`
- CT-120: Deve retornar null quando o email não está em uso
- CT-121: Deve retornar o usuário quando o email já existe
- CT-122: Deve ignorar o próprio Id na verificação (para edição)
