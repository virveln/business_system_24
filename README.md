# Affärssystem Del 1
Denna laboration går ut på att både designa och implementera ett affärssystem. Affärssystemet ska användas till en fysisk affär som säljer media (t.ex. böcker, cd-skivor, spel). Systemet ska användas av anställda i affären och kunna hantera både kassahantering och lagerhållning.

## Kravspecifikation
Det inlämnade programmet ska:
* Vara implementerade i C#.NET med UWP, WPF eller WinForms som grafiskt bibliotek.
* Vara implementerade i .NET 6.0 eller nyare.
* Ska gå att köra på en svensk dator, med ett svenskt operativsystem.
* Ska inte under rimliga omständigheter krasha. Vi kommer att monkey testa alla program, men vi kommer inte göra underliga ändringar i exekveringsmiljön eller liknande. Hantera alla fel med exceptions.
* Vara användarvänliga, med tydliga och beskrivande felmeddelanden, och inte bryta allvarligt mot Microsofts rekommendationer för gränssnittsprogrammering.

Följande funktionalitet ska finnas i systemet:
1. Lägga till nya produkter.
2. Ta bort produkter.
3. Lägga till en leverans av en eller flera produkter från grossist. Här räcker det med att antalet uppdateras, leveransen behöver inte loggas.
4. Försäljning av produkt

Systemet ska ha två separata vyer (t.ex. flikar eller fönster), en för lagerarbete och en för kassabruk (dock max en instans av varje vy). Vyerna ska båda ha en lättöverskådlig produktlista, och endast tillgång till relevant funktionalitet. Vyerna ska arbeta mot samma data som ni ska lagra i en CSV-fil. Vid försäljning ska varor kunna läggas till i en kundkorg innan köpet genomförs. Vid uppstart av programmet ska denna fil läsas in, och när programmet stängs av ska filen sparas. Ni som använder UWP skall låta användaren av programmet välja fil med hjälp av FileOpenPicker då filhanteringen i UWP skiljer sig en hel del från WinForms och WPF. Databasen (CSV-filen) ska ligga i samma katalogstruktur som er källkod (OBS! Gäller INTE er som använder UWP) så att den följer med när ni zippar projektet vid inlämning (inga hårdkodade absoluta sökvägar, men hårdkodade relativa sökvägar är ok). Kundkorgen är tillfällig och behöver inte sparas till fil.

Tänk på att göra systemet så generellt som möjligt, och se till att det är estetiskt tilltalande och användarvänligt för butikspersonal utan dataerfarenhet.

Kontrollfunktioner ska finnas så att:

* Om någon försöker lägga till en ny vara i sortimentet med ett redan upptaget varunummer ska detta ej godkännas.
* Om någon i personalen försöker ta bort en vara ur sortimentet, och dess lagerstatus inte är noll, ska en dialogruta dyka upp om man verkligen vill ta bort varan. I dialogrutan ska det gå att svara Ja eller Nej.
* Man ska inte kunna sälja varor som inte finns på lager.
* Produkter får inte kunna ha felaktig information (exempelvis avsaknad av obligatoriskt fält, negativa priser, bokstäver i varunumret, en speltid som är negativ eller inte en siffra)
* Varje varunummer ska vara unikt.  

Produkterna i affären ska vara följande från start (* = obligatoriskt fält, grå bakgrund = ingen data från grossist):

### Böcker
| Namn*                       | Pris* | Författare             | Genre         | Format    | Språk  |
|-----------------------------|-------|------------------------|---------------|-----------|--------|
| Bello Gallico et Civili     | 449   | Julius Caesar          | Historia      | Inbunden  | Latin  |
| How to Read a Book          | 149   | Mortimer J. Adler      | Kursliteratur | Pocket    |        |
| Moby Dick                   | 49    | Herman Melville        | Äventyr       | Pocket    |        |
| Great Gatsby                | 79    | F. Scott Fitzgerald    | Noir          | E-Bok     |        |
| House of Leaves             | 49    | Mark Z. Danielewski    | Skräck        |           |        |

### Dataspel
| Namn*                     | Pris* | Plattform       |
|---------------------------|-------|-----------------|
| Elden Ring                | 599   | Playstation 5   |
| Demon's Souls             | 499   | Playstation 5   |
| Microsoft Flight Simulator| 499   | PC              |
| Planescape Torment        | 99    | PC              |
| Disco Elysium             | 399   | PC              |

### Filmer
| Namn*                    | Pris* | Format  | Speltid  |
|--------------------------|-------|---------|----------|
| Nyckeln till frihet      | 99    | DVD     | 142 min  |
| Gudfadern                | 99    | DVD     | 152 min  |
| Konungens återkomst      | 199   | Blu-Ray | 154 min  |
| Pulp fiction             | 199   | Blu-Ray |          |
| Schindlers list          | 99    | DVD     |          |

# Affärssystem Del 2: integrera med API
Laborationen består av att integrera din butik med ett centrallager som exponeras mot webben. Programmet ska i grundversionen synkronisera lagerstatus och pris mot centrallagret, och i de avancerade versionerna även kunna uppdatera centrallagret, samt notera historisk data.

## Beskrivning av API:et
När man anropar hemsidan så returnerar webbservern vilka produkter som finns i centrallagret. Om anropet lyckats är svaret indelat i två delar: metadata och produkter.
Här behöver ni fylla i <id> och <stock> med produktens ni vill uppdateras id respektive en ny lagerstatus för produkten. Eftersom ni är flera studenter som kommer åt samma API, kan det ibland dröja innan man får ett svar, så var vaksam på detta.

## Kravspecifikation
Ni ska implementera en knapp i programmet som laddar ned data från centrallagret, och uppdaterar produkterna i ert lokala lager. Endast pris och lagerstatus behöver uppdateras. Om API:et återger ett felmeddelande, behöver detta hanteras och visas upp för användaren. Ni behöver inte uppdatera åt andra hållet.

# Mockup
![affarssystem1.png](https://github.com/virveln/business_system_24/blob/main/mockup/affarssystem1.png)  
*Vy för lagerhantering*

![affarssystem2.png](https://github.com/virveln/business_system_24/blob/main/mockup/affarssystem2.png)  
*Vy för att lägga till ny produkt i systemet*

![affarssystem3.png](https://github.com/virveln/business_system_24/blob/main/mockup/affarssystem3.png)  
*Vy för lagerhantering*

![affarssystem4.png](https://github.com/virveln/business_system_24/blob/main/mockup/affarssystem4.png)  
*Vy för att se vad som finns i varukorgen*
