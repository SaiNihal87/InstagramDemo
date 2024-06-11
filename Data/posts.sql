DROP TABLE IF EXISTS posts;
CREATE TABLE "posts" (
    "id" SERIAL8 PRIMARY KEY,
    "user_id" int8,
    "title" VARCHAR(255) NOT NULL,
    "content" VARCHAR(255) NOT NULL,
    "likes" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ,
    CONSTRAINT fk_users
    FOREIGN KEY(user_id) 
      REFERENCES users(id)
      ON DELETE CASCADE,
);