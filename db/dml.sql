-- =====================================================
-- DATOS INICIALES
-- =====================================================

-- Insertar géneros
INSERT IGNORE INTO gender (gender_id, name) VALUES
(1, 'Masculino'),
(2, 'Femenino'),
(3, 'No binario');

-- Insertar orientaciones sexuales
INSERT IGNORE INTO orientation (orientation_id, name) VALUES
(1, 'Heterosexual'),
(2, 'Homosexual'),
(3, 'Bisexual'),
(4, 'Pansexual');

-- Insertar carreras/profesiones
INSERT IGNORE INTO career (career_id, name, category) VALUES
(1, 'Ingeniería de Sistemas', 'Ingeniería'),
(2, 'Ingeniería Informática', 'Ingeniería'),
(3, 'Ingeniería Civil', 'Ingeniería'),
(4, 'Medicina', 'Ciencias de la Salud'),
(5, 'Enfermería', 'Ciencias de la Salud'),
(6, 'Odontología', 'Ciencias de la Salud'),
(7, 'Derecho', 'Ciencias Sociales'),
(8, 'Psicología', 'Ciencias Sociales'),
(9, 'Administración de Empresas', 'Ciencias Sociales'),
(10, 'Contabilidad', 'Ciencias Sociales'),
(11, 'Matemáticas', 'Ciencias'),
(12, 'Física', 'Ciencias'),
(13, 'Química', 'Ciencias'),
(14, 'Biología', 'Ciencias'),
(15, 'Arquitectura', 'Arte y Diseño'),
(16, 'Diseño Gráfico', 'Arte y Diseño'),
(17, 'Música', 'Arte y Diseño');

-- Insertar intereses
INSERT IGNORE INTO interest (interest_id, name, category) VALUES
(1, 'Fútbol', 'Deportes'),
(2, 'Baloncesto', 'Deportes'),
(3, 'Tenis', 'Deportes'),
(4, 'Rock', 'Música'),
(5, 'Pop', 'Música'),
(6, 'Jazz', 'Música'),
(7, 'Pintura', 'Arte'),
(8, 'Fotografía', 'Arte'),
(9, 'Escultura', 'Arte'),
(10, 'Programación', 'Tecnología'),
(11, 'Videojuegos', 'Tecnología'),
(12, 'Inteligencia Artificial', 'Tecnología'),
(13, 'Cocina', 'Gastronomía'),
(14, 'Viajes', 'Aventura'),
(15, 'Lectura', 'Cultura');

-- Insertar usuario administrador
INSERT IGNORE INTO user (user_id, fullname, email, password_hash, birthdate, gender_id, orientation_id, career_id, profile_phrase) VALUES
(1, 'Administrador', 'admin@campuslove.com', '$2a$12$p1Ci.PAhLTtw8j1SL6Vy5e.XegdIYZHQMDT0CeSvAs1L0f3qbGpP2', '1990-01-01', 1, 1, 1, 'Administrador del sistema');

-- Insertar algunos usuarios de prueba
INSERT IGNORE INTO user (user_id, fullname, email, password_hash, birthdate, gender_id, orientation_id, career_id, profile_phrase) VALUES

(2, 'andres', 'andres@gmail.com', '$2a$12$csrjqC/Yph87SqfBzNJAbeuRDvOjFMHE9fI9Ln9eTgQ19yu/ClfAq', '1994-01-01', 1, 1, 1, 'Usuario de prueba'),
(3, 'EDWIN', 'edwin@gmail.com', '$2a$12$rU3Ru3za7XPI9Vp8vd9pT.vxYNAGBG7x4spPeBi4XHLqwKfdzA00e', '1994-01-01', 1, 1, 1, 'Usuario de prueba');