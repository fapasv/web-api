--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2
-- Dumped by pg_dump version 14.2

-- Started on 2022-04-19 10:49:25

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3327 (class 1262 OID 16453)
-- Name: librosdb; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE librosdb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Spanish_El Salvador.1252';


\connect librosdb

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_table_access_method = heap;

--
-- TOC entry 213 (class 1259 OID 16477)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


--
-- TOC entry 210 (class 1259 OID 16457)
-- Name: autor; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.autor (
    id integer NOT NULL,
    nombre character varying NOT NULL,
    alias character varying
);


--
-- TOC entry 209 (class 1259 OID 16456)
-- Name: autor_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.autor_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3328 (class 0 OID 0)
-- Dependencies: 209
-- Name: autor_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.autor_id_seq OWNED BY public.autor.id;


--
-- TOC entry 212 (class 1259 OID 16466)
-- Name: libro; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.libro (
    id integer NOT NULL,
    titulo character varying NOT NULL,
    autor_id integer NOT NULL,
    fecha_publicacion date,
    precio numeric(12,2) DEFAULT 0.0 NOT NULL
);


--
-- TOC entry 211 (class 1259 OID 16465)
-- Name: libro_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.libro_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3329 (class 0 OID 0)
-- Dependencies: 211
-- Name: libro_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.libro_id_seq OWNED BY public.libro.id;


--
-- TOC entry 3173 (class 2604 OID 16460)
-- Name: autor id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.autor ALTER COLUMN id SET DEFAULT nextval('public.autor_id_seq'::regclass);


--
-- TOC entry 3174 (class 2604 OID 16469)
-- Name: libro id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.libro ALTER COLUMN id SET DEFAULT nextval('public.libro_id_seq'::regclass);


--
-- TOC entry 3181 (class 2606 OID 16481)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3177 (class 2606 OID 16464)
-- Name: autor autor_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.autor
    ADD CONSTRAINT autor_pkey PRIMARY KEY (id);


--
-- TOC entry 3179 (class 2606 OID 16484)
-- Name: libro libro_pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.libro
    ADD CONSTRAINT libro_pk PRIMARY KEY (id);


--
-- TOC entry 3182 (class 2606 OID 16472)
-- Name: libro fk_autor; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.libro
    ADD CONSTRAINT fk_autor FOREIGN KEY (autor_id) REFERENCES public.autor(id);


-- Completed on 2022-04-19 10:49:26

--
-- PostgreSQL database dump complete
--

