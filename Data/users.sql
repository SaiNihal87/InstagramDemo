DROP TABLE IF EXISTS users;
CREATE TABLE "users" (
    "id" SERIAL8 PRIMARY KEY,
    "username" VARCHAR(20) NOT NULL,
    "full_name" VARCHAR(255) NOT NULL,
    "password" VARCHAR(20) NOT NULL,
    "email" VARCHAR(255) ,
    "phone" INT8,
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ 
);