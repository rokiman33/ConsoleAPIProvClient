using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace RestClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //genero y obtengo el token
            string token = await GetBearerToken();
            if (!string.IsNullOrEmpty(token))
            {
                //Console.WriteLine("Bearer Token: " + token);
                //Enviar documento en formato XML o JSON
                //var result = await LoadDocument(token);
                
                //Enviar documento en formato CSV o excel
                await UploadFileAsync("C:\\doctos\\file.csv", token,"Documento de Pruebas");

               // Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Failed to retrieve token.");
            }
        }

        
        //obtener el token
        static async Task<string> GetBearerToken()
        {
            string baseUrl = "https://localhost/api/v1/token";
            HttpClient client = new HttpClient();

            try
            {
                //se enviar acceso para generar el token
                var requestBody = new StringContent(
                    "{\"rut\": \"1-9\", \"passwordKey\": \"\"}",
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = await client.PostAsync(baseUrl, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JObject.Parse(responseBody);
                    return responseJson["data"]["bearerToken"].ToString();
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: " + e.Message);
                return null;
            }
        }
        
        //Enviar Archivo xml
        static async Task<string> LoadDocument(string bearerToken)
        {
            string url = "https://localhost/Document/";
            string xmlContent = "<DTE version='1.0'><Documento ID='2'><Encabezado><IdDoc><TipoDTE>39</TipoDTE><Folio>748</Folio><FchEmis>2014-12-13</FchEmis><IndServicio>1</IndServicio><IndMntNeto>2</IndMntNeto><PeriodoDesde>2012-12-13</PeriodoDesde><PeriodoHasta>2012-12-13</PeriodoHasta><FchVenc>2012-12-13</FchVenc></IdDoc><Emisor><RUTEmisor>str1234</RUTEmisor><RznSocEmisor>str1234</RznSocEmisor><GiroEmisor>str1234</GiroEmisor><CdgSIISucur>745</CdgSIISucur><DirOrigen>str1234</DirOrigen><CmnaOrigen>str1234</CmnaOrigen><CiudadOrigen>str1234</CiudadOrigen></Emisor><Receptor><RUTRecep>str1234</RUTRecep><CdgIntRecep>str1234</CdgIntRecep><RznSocRecep>str1234</RznSocRecep><Contacto>str1234</Contacto><DirRecep>str1234</DirRecep><CmnaRecep>str1234</CmnaRecep><CiudadRecep>str1234</CiudadRecep><DirPostal>str1234</DirPostal><CmnaPostal>str1234</CmnaPostal><CiudadPostal>str1234</CiudadPostal></Receptor><Totales><MntNeto>33</MntNeto><MntExe>33</MntExe><IVA>745</IVA><MntTotal>33</MntTotal><MontoNF>1234</MontoNF><TotalPeriodo>33</TotalPeriodo><SaldoAnterior>1234</SaldoAnterior><VlrPagar>1234</VlrPagar></Totales></Encabezado><Detalle><NroLinDet>745</NroLinDet><CdgItem><TpoCodigo>str1234</TpoCodigo><VlrCodigo>str1234</VlrCodigo></CdgItem><IndExe>1</IndExe><ItemEspectaculo>01</ItemEspectaculo><RUTMandante>str1234</RUTMandante><NmbItem>str1234</NmbItem><InfoTicket><FolioTicket>745</FolioTicket><FchGenera>2012-12-13T12:12:12</FchGenera><NmbEvento>str1234</NmbEvento><TpoTiket>str1234</TpoTiket><CdgEvento>str12</CdgEvento><FchEvento>2012-12-13T12:12:12</FchEvento><LugarEvento>str1234</LugarEvento><UbicEvento>str1234</UbicEvento><FilaUbicEvento>str</FilaUbicEvento><AsntoUbicEvento>str</AsntoUbicEvento></InfoTicket><DscItem>str1234</DscItem><QtyItem>123.45</QtyItem><UnmdItem>str1</UnmdItem><PrcItem>123.45</PrcItem><DescuentoPct>123.45</DescuentoPct><DescuentoMonto>33</DescuentoMonto><RecargoPct>123.45</RecargoPct><RecargoMonto>33</RecargoMonto><MontoItem>33</MontoItem></Detalle><SubTotInfo><NroSTI>745</NroSTI><GlosaSTI>str1234</GlosaSTI><OrdenSTI>745</OrdenSTI><SubTotNetoSTI>123.45</SubTotNetoSTI><SubTotIVASTI>123.45</SubTotIVASTI><SubTotAdicSTI>123.45</SubTotAdicSTI><SubTotExeSTI>123.45</SubTotExeSTI><ValSubtotSTI>123.45</ValSubtotSTI><LineasDeta>745</LineasDeta></SubTotInfo><DscRcgGlobal><NroLinDR>745</NroLinDR><TpoMov>D</TpoMov><GlosaDR>str1234</GlosaDR><TpoValor>%</TpoValor><ValorDR>123.45</ValorDR><IndExeDR>1</IndExeDR></DscRcgGlobal><Referencia><NroLinRef>745</NroLinRef><CodRef>str1234</CodRef><RazonRef>str1234</RazonRef><CodVndor>str1234</CodVndor><CodCaja>str1234</CodCaja></Referencia></Documento></DTE>"; // Tu XML va aquí
            var requestBody = new StringContent(
                $"{{\"contentDocument\": \"{xmlContent}\", \"typeDocument\": \"VENTAS\", \"format\": \"XML\", \"docTypeDescription\": \"FACTURA VENTAS\"}}",
                Encoding.UTF8,
                "application/json");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                HttpResponseMessage response = await client.PostAsync(url, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
        }

        //Enviar Archivo csv
        public static async Task UploadFileAsync(string filePath, string bearerToken, string description)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = $"https://localhost/loadFile/upload?description={description}";
                
                using var multipartFormContent = new MultipartFormDataContent();
                multipartFormContent.Add(new StringContent("OTROS"), "doctype");
                multipartFormContent.Add(new ByteArrayContent(await System.IO.File.ReadAllBytesAsync(filePath))
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("text/csv")
                    }
                }, "file", System.IO.Path.GetFileName(filePath));

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var response = await client.PostAsync(url, multipartFormContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Success: {responseContent}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }



    }
}
