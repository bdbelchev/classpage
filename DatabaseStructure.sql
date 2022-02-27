CREATE SCHEMA schooldb;
USE schooldb;

CREATE TABLE teachers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(45) NOT NULL,
    middle_name VARCHAR(45),
    last_name VARCHAR(45) NOT NULL,
    email VARCHAR(45) NOT NULL UNIQUE,
    phone CHAR(9) UNIQUE
);

CREATE TABLE subjects (
    id INT PRIMARY KEY AUTO_INCREMENT,
    subject_name VARCHAR(45) NOT NULL UNIQUE
);

CREATE TABLE teachers_subjects (
    teacher_id INT NOT NULL,
    subject_id INT NOT NULL,
    CONSTRAINT pk_teachers_subjects PRIMARY KEY (teacher_id , subject_id),
    FOREIGN KEY (teacher_id)
        REFERENCES teachers (id),
    FOREIGN KEY (subject_id)
        REFERENCES subjects (id)
);

CREATE TABLE classes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    class_name VARCHAR(3) NOT NULL UNIQUE,
    main_teacher_id INT NOT NULL UNIQUE,
    CONSTRAINT fk_classes_teachers_main FOREIGN KEY (main_teacher_id)
        REFERENCES teachers (id)
);

CREATE TABLE students (
    id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(45) NOT NULL,
    middle_name VARCHAR(45),
    last_name VARCHAR(45) NOT NULL,
    class_id INT NOT NULL,
    email VARCHAR(45) NOT NULL UNIQUE,
    phone CHAR(9) UNIQUE,
    CONSTRAINT fk_students_classes FOREIGN KEY (class_id)
        REFERENCES classes (id)
);

CREATE TABLE classes_teachers (
    class_id INT NOT NULL,
    teacher_id INT NOT NULL,
    CONSTRAINT pk_classes_teachers PRIMARY KEY (class_id , teacher_id),
    FOREIGN KEY (class_id)
        REFERENCES classes (id),
    FOREIGN KEY (teacher_id)
        REFERENCES teachers (id)
);

CREATE TABLE grades (
    id INT PRIMARY KEY AUTO_INCREMENT,
    value DOUBLE NOT NULL,
    subject_id INT NOT NULL,
    teacher_id INT NOT NULL,
    student_id INT NOT NULL,
    date_added DATE NOT NULL,
    description TEXT,
    CONSTRAINT fk_grades_subjects FOREIGN KEY (subject_id)
        REFERENCES subjects (id),
    CONSTRAINT fk_grades_teachers FOREIGN KEY (teacher_id)
        REFERENCES teachers (id),
    CONSTRAINT fk_grades_students FOREIGN KEY (student_id)
        REFERENCES students (id)
);
