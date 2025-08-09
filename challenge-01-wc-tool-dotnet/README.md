# Coding Challenged 1: WC Tool

El reto se describe en [Coding Challenges: 01](https://www.linkedin.com/pulse/coding-challenge-1-john-crickett/?trackingId=a7bfnSiSQuejPO7k9wHolA%3D%3D)

## Desarrollo

Vamos a crear el comando **WC** en **.NET**, en su versión 8.0 del SDK.

## Funcionamiento

Nuestra versión del comando **WC** funcionará de la siguiente manera:

```
NAME
       wc - print newline, word, and byte counts for each file

SYNOPSIS
       wc [OPTION]... [FILE]...

DESCRIPTION
       Print newline, word, and byte counts for each FILE, and a total line if
       more  than one FILE is specified.  A word is a non-zero-length sequence
       of printable characters delimited by white space.

       With no FILE, or when FILE is -, read standard input.

       The options below may be used to select which counts are  printed,  al‐
       ways  in  the  following order: newline, word, character, byte, maximum
       line length.

       All character processing in this version assumes ASCII encoding. That is,
       each character is considered to be exactly one byte, and multibyte characters
       (such as those in UTF-8) will be counted as multiple individual characters.

OPTIONS

       -c, --bytes
              Print the byte counts.

       -m, --chars
              Print the character counts (in ASCII: one byte per character).

       -l, --lines
              Print the newline counts.

       -L, --max-line-length
              Print the maximum display width (in ASCII characters per line).

       -w, --words
              Print the word counts, assuming words are separated by ASCII whitespace.

```

---

## Limitaciones

Se va a limitar el comando para que **no interpete las siguientes opciones**:

```
--files0-from=F
    Read input from the files specified by NUL-terminated names in file "F"; If "F" is - then read names from standard input

--total=WHEN
    When to print a line with total counts; WHEN can be: auto, always, only, never

--help
    Display this help and exit

--version
    Output version information and exit
```

También se limita la codficación de caracteres a ASCII en esta primera versión.

---

## Lectura de Argumentos

El número y orden de los argumentos pueden ser diferentes:

Ejemplos válidos:

- Cuenta líneas, palabras y caracteres de "file.txt"
```bash
wc file.txt
```

- Procesa múltiples archivos
```bash
wc ./file1.txt file2.html ../file3.png
```

- Cuenta cracteres de "file.txt"
```bash
wc -c file.txt
```

- Cuenta caracteres y palabras de "file.txt"
```bash
wc -c -w file.txt
```

- Cuenta caracteres y palabras, con opciones combinadas
```bash
wc -cw file.txt
```

- Opción larga (--arg) para contar palabras del archivo
```bash
wc --words file.txt
```

- Combina opciones largas (--words) y cortas (-c) de varios archivos.
```bash
wc --words -c file.png file.txt
```

- Combina opciones largas (--words) y cortas (-cl) de varios archivos.
```bash
wc --words -cl file.png file.txt
```

- Combina opciones largas (--words) y cortas (-c -l) de varios archivos.
```bash
wc --words -c -l -- -file.png
```

- Utiliza el indicador de fin de opciones (--) para interpretar el archivo como tal, y no como un argumento (-file.txt)
```bash
wc -- -file.txt
```

- Utiliza el indicador de fin de opciones (--) para interpretar el archivo como tal, y no como un argumento (-file.txt)
```bash
wc --words -cl -- -file.png
```