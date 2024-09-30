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
insert into user (username, email) values ("ropf", "ropf@itu.com");
insert into user (username, email) values ("kegr", "kegr@itu.com");
insert into user (username, email) values ("jgjo", "jgjo@itu.com");
insert into message (author_id, text, pub_date) values (1, "Test message 1", 1234567890);
insert into message (author_id, text, pub_date) values (1, "Test message 2", 1234567890);
insert into message (author_id, text, pub_date) values (1, "Test message 3", 1234567890);
insert into message (author_id, text, pub_date) values (2, "Test message 1", 1234888890);
insert into message (author_id, text, pub_date) values (2, "Test message 2", 1234888890);
insert into message (author_id, text, pub_date) values (2, "Test message 3", 1234888890);
insert into message (author_id, text, pub_date) values (3, "Test message 1", 1234888891);
insert into message (author_id, text, pub_date) values (3, "Test message 2", 1234888891);
insert into message (author_id, text, pub_date) values (3, "Test message 3", 1234888891);
