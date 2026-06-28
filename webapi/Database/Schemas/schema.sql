CREATE TABLE "usuarios" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_usuarios" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "Cpf" TEXT NOT NULL,
    "AddressId" INTEGER NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_usuarios_enderecos_AddressId" FOREIGN KEY ("AddressId") REFERENCES "enderecos" ("Id")
);

CREATE TABLE "planos_saude" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_planos_saude" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Coverage" TEXT NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

CREATE TABLE "medicos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_medicos" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "CPF" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "CRM" TEXT NOT NULL,
    "SpecializationId" INTEGER NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_medicos_especializacoes_SpecializationId" FOREIGN KEY ("SpecializationId") REFERENCES "especializacoes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "exames" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_exames" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Active" INTEGER NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

CREATE TABLE "especializacoes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_especializacoes" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

CREATE TABLE "enderecos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_enderecos" PRIMARY KEY AUTOINCREMENT,
    "Logradouro" TEXT NOT NULL,
    "Cep" TEXT NULL,
    "Bairro" TEXT NULL,
    "Cidade" TEXT NULL,
    "Estado" TEXT NULL,
    "Pais" TEXT NULL,
    "Numero" TEXT NULL,
    "Complemento" TEXT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

CREATE TABLE "convenios" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_convenios" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Provider" TEXT NOT NULL,
    "StartDate" TEXT NOT NULL,
    "EndDate" TEXT NULL,
    "HealthPlanId" INTEGER NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_convenios_planos_saude_HealthPlanId" FOREIGN KEY ("HealthPlanId") REFERENCES "planos_saude" ("Id")
;);

CREATE TABLE "centros_medicos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_centros_medicos" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "PhoneNumber" TEXT NOT NULL,
    "Email" TEXT NULL,
    "AddressId" INTEGER NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_centros_medicos_enderecos_AddressId" FOREIGN KEY ("AddressId") REFERENCES "enderecos" ("Id") ON DELETE CASCADE
);

CREATE TABLE "agendamentos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_agendamentos" PRIMARY KEY AUTOINCREMENT,
    "Date" TEXT NOT NULL,
    "Protocol" TEXT NOT NULL,
    "Notes" TEXT NULL,
    "Status" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL,
    "DoctorId" INTEGER NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_agendamentos_medicos_DoctorId" FOREIGN KEY ("DoctorId") REFERENCES "medicos" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_agendamentos_usuarios_UserId" FOREIGN KEY ("UserId") REFERENCES "usuarios" ("Id") ON DELETE CASCADE
);

CREATE TABLE "agendamento_avaliacoes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_agendamento_avaliacoes" PRIMARY KEY AUTOINCREMENT,
    "Rating" INTEGER NOT NULL,
    "Comment" TEXT NULL,
    "AppointmentId" INTEGER NOT NULL,
    "UserId" INTEGER NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_agendamento_avaliacoes_agendamentos_AppointmentId" FOREIGN KEY ("AppointmentId") REFERENCES "agendamentos" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_agendamento_avaliacoes_usuarios_UserId" FOREIGN KEY ("UserId") REFERENCES "usuarios" ("Id") ON DELETE CASCADE
);

CREATE TABLE "administradores" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_administradores" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "CreatedAt" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

