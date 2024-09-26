-- create tables
drop table if exists user;
create table user (
  user_id integer primary key autoincrement,
  username string not null,
  email string not null
);

drop table if exists message;
create table message (
  message_id integer primary key autoincrement,
  author_id integer not null,
  text string not null,
  pub_date integer
);

-- add test data
insert into user (username, email) values ("aabb", "ab@ab.com");
-- insert into message (author_id, text, pub_date) values (1, "hello", 1234567890);
-- insert into message (author_id, text, pub_date) values (1, "Test message 2", 1234888890);
-- insert into message (author_id, text, pub_date) values (1, "Test message but 3", 1234888891);
