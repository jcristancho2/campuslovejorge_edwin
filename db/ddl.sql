CREATE DATABASE IF NOT EXISTS examendb


USE examendb;

-- =====================================================
-- TABLAS DE CATÁLOGO
-- =====================================================

-- Tabla de géneros
CREATE TABLE IF NOT EXISTS gender (
    gender_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de orientaciones sexuales
CREATE TABLE IF NOT EXISTS orientation (
    orientation_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de carreras/profesiones
CREATE TABLE IF NOT EXISTS career (
    career_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    category VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_category (category)
);

-- Tabla de intereses
CREATE TABLE IF NOT EXISTS interest (
    interest_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    category VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_category (category)
);

-- =====================================================
-- TABLA PRINCIPAL DE USUARIOS
-- =====================================================

CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    fullname VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL,
    gender_id INT NOT NULL,
    orientation_id INT NOT NULL,
    career_id INT NOT NULL,
    profile_phrase VARCHAR(500),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (gender_id) REFERENCES gender(gender_id) ON DELETE RESTRICT,
    FOREIGN KEY (orientation_id) REFERENCES orientation(orientation_id) ON DELETE RESTRICT,
    FOREIGN KEY (career_id) REFERENCES career(career_id) ON DELETE RESTRICT,
    
    INDEX idx_email (email),
    INDEX idx_birthdate (birthdate),
    INDEX idx_gender (gender_id),
    INDEX idx_orientation (orientation_id),
    INDEX idx_career (career_id)
);

-- =====================================================
-- TABLAS DE RELACIONES
-- =====================================================

-- Tabla de intereses de usuario (relación muchos a muchos)
CREATE TABLE IF NOT EXISTS user_interest (
    user_id INT NOT NULL,
    interest_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (user_id, interest_id),
    FOREIGN KEY (user_id) REFERENCES user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (interest_id) REFERENCES interest(interest_id) ON DELETE CASCADE
);

-- Tabla de likes entre usuarios
CREATE TABLE IF NOT EXISTS user_like (
    like_id INT AUTO_INCREMENT PRIMARY KEY,
    liker_id INT NOT NULL,
    liked_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (liker_id) REFERENCES user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (liked_id) REFERENCES user(user_id) ON DELETE CASCADE,
    
    UNIQUE KEY unique_like (liker_id, liked_id),
    INDEX idx_liker (liker_id),
    INDEX idx_liked (liked_id),
    INDEX idx_created (created_at)
);

-- Tabla de límites diarios de likes
CREATE TABLE IF NOT EXISTS daily_likes (
    daily_like_limit_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    likes_used INT NOT NULL DEFAULT 0,
    max_likes_per_day INT NOT NULL DEFAULT 10,
    date DATE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (user_id) REFERENCES user(user_id) ON DELETE CASCADE,
    
    UNIQUE KEY unique_user_date (user_id, date),
    INDEX idx_user_date (user_id, date)
);

-- Tabla de matches
CREATE TABLE IF NOT EXISTS match_table (
    match_id INT AUTO_INCREMENT PRIMARY KEY,
    user1_id INT NOT NULL,
    user2_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (user1_id) REFERENCES user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (user2_id) REFERENCES user(user_id) ON DELETE CASCADE,
    
    UNIQUE KEY unique_match (user1_id, user2_id),
    INDEX idx_user1 (user1_id),
    INDEX idx_user2 (user2_id),
    INDEX idx_created (created_at)
);