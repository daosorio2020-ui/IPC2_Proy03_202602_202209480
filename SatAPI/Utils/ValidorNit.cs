namespace SatAPI.Utils;

public static class ValidadorNit
{
    public static bool EsValido(string? nit)
    {
        if (string.IsNullOrWhiteSpace(nit) || nit.Length < 2) return false;

        nit = nit.ToUpper();
        // Separar la cadena: el último caracter es el validador (V), el resto es el número base (X)
        char digitoVerificador = nit[^1]; // Equivalente a nit.Last()
        string baseNit = nit.Substring(0, nit.Length - 1);

        int suma = 0;
        // El validador es la posición 1 (más a la derecha).
        // Empezamos a multiplicar el número base desde la posición 2 de derecha a izquierda[span_3](start_span)[span_3](end_span).
        int posicion = 2;

        for (int i = baseNit.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(baseNit[i])) return false; // Validar que sean números
           
            suma += int.Parse(baseNit[i].ToString()) * posicion;
            posicion++;
        }

        // 3. Obtenga el módulo 11 de la sumatoria[span_4](start_span)[span_4](end_span).
        int paso3 = suma % 11;
       
        // 4. A 11 réstale el resultado obtenido en el punto 3[span_5](start_span)[span_5](end_span).
        int paso4 = 11 - paso3;
       
        // 5. Calcule el módulo 11 del resultado obtenido en el punto No. 4[span_6](start_span)[span_6](end_span).
        int resultadoFinal = paso4 % 11;

        // Si el resultado es 10, el dígito debe ser K; si no, es el número obtenido[span_7](start_span)[span_7](end_span).
        char validadorEsperado = resultadoFinal == 10 ? 'K' : resultadoFinal.ToString()[0];

        return digitoVerificador == validadorEsperado;
    }
}