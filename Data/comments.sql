DROP TABLE IF EXISTS comments;
CREATE TABLE "comments" (
    "id" SERIAL8 PRIMARY KEY,
    "post_id" int8,
    "user_id" int8,
    "description" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ,
    CONSTRAINT fk_users
    FOREIGN KEY(user_id) 
      REFERENCES users(id)
      ON DELETE CASCADE,
    FOREIGN KEY(post_id) 
      REFERENCES posts(id)
      ON DELETE CASCADE
);