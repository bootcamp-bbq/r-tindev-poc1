CREATE TABLE public.developers
(
    id character(40) NOT NULL,
    name character,
    "user" character,
    github_url character,
    created_at date,
    bio character,
    avatar_url character,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.developers
    OWNER to postgres;