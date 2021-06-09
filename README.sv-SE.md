🌍[English](README.md) ∙ [Swedish](README.sv-SE.md)

# WeekNumber
Windows 10 systemfältsapplikation som visar aktuellt veckonummer

<img src="https://user-images.githubusercontent.com/2292809/120221521-d6a79580-c23e-11eb-99d1-be6210b43fcf.png" data-canonical-src="https://user-images.githubusercontent.com/2292809/120221521-d6a79580-c23e-11eb-99d1-be6210b43fcf.png" alt="WeekNumber" width="150" height="150" /> ![image](https://user-images.githubusercontent.com/2292809/121431601-e8451780-c979-11eb-9734-f30304c348d1.png)

[![Latest release ZIP](https://img.shields.io/github/v/release/voltura/WeekNumber?label=ladda%20ner%20senaste%20versionen&style=for-the-badge)](https://github.com/voltura/weeknumber/releases/latest/download/WeekNumber.zip)

[![Github All Releases](https://img.shields.io/github/downloads/voltura/WeekNumber/total.svg)]()
[![License](https://img.shields.io/badge/licence-MIT-green)]()

## Funktioner
Aktuellt veckonummer visas som en ikon i systemfältet i Windows aktivitetsfält.
Slå upp veckonummer för andra datum genom att dubbel-klicka på ikonen.

Ställ in att starta med Windows, anpassa ikonfärger, språk, notiser, automatiska uppdatering, kalender-regler och mycket mer. För detaljerad beskrivning se _**Hjälpavsnitt**_ nedan.

### Språkstöd
- Engelska
- Svenska

<sub>_Om du vill bidra med översättning till ett annat språk, vänligen skapa ett ärende på GitHub så kontaktar jag dig!_<sub>

## Skärmbilder
#### Ikon med standard-färger
![Taskbar icon](https://user-images.githubusercontent.com/2292809/120904782-473f1f80-c64e-11eb-9256-c3d0ddab2124.png)

#### Högerklicks-meny
![Högerklicks-menu](https://user-images.githubusercontent.com/2292809/121432228-b08a9f80-c97a-11eb-9407-c79071133417.png)

#### Ta reda på veckonummer för valfritt datum
![Weeknumber formulär](https://user-images.githubusercontent.com/2292809/121432297-c730f680-c97a-11eb-9530-005d5062039b.png)

#### Inställningsmeny med språkval
![Inställningsmeny](https://user-images.githubusercontent.com/2292809/121432466-00696680-c97b-11eb-8f34-797737331f33.png)

#### Exempel på ikoner med anpassade färger
![Custom colors #1](https://user-images.githubusercontent.com/2292809/118048718-f4d74f80-b37c-11eb-8b36-211250ff25c5.png) ![Custom colors #2](https://user-images.githubusercontent.com/2292809/120920791-e997eb00-c6c0-11eb-889e-9a3e67787033.png) ![Custom colors #3](https://user-images.githubusercontent.com/2292809/120921288-498f9100-c6c3-11eb-8451-d2c3e19fba30.png)

#### Färgval, med möjlighet att definiera och nyttja egendefinierade färger; både ikonens text och bakgrundsfärg kan ändras
![Color picker](https://user-images.githubusercontent.com/2292809/118050315-4e407e00-b37f-11eb-8ac9-17cc1a08aa08.png)

#### Mera höger-klicksmenyval
![Mera inställningar](https://user-images.githubusercontent.com/2292809/121432695-49b9b600-c97b-11eb-8576-89b792cd2514.png)
   
## Installation
Ladda ner installationsprogrammet och kör det för att installera, applikationen startas efter installation.
Om du laddat ned WeekNumber.zip via *Download latest release* packa först upp arkivet och kör sedan installationsprogrammet.
För att ta bort applikationen så kör avinstallationsprogrammet eller använd dig av Windows *Lägg till/ta bort program*-funktion. Det går också att avinstallera via Windows kontrollpanel under sektionen *Program och funktioner*. 

### Installationsnotiser
Välj att behålla filen som du laddat ner, ser ut liknande nedan (beroende av vilken webbläsare, språk och övriga inställningar du har)

![Keep file if prompted #1](https://user-images.githubusercontent.com/2292809/120716901-fa7d0c80-c4c6-11eb-9232-f279f959f0a6.png)

![Keep file if prompted #2](https://user-images.githubusercontent.com/2292809/118524536-8c9eba00-b73e-11eb-9c6c-bc8defde0caa.png)

Använd nedanstående operationer för att tillåta installationsprogrammet att köras på din PC då _Microsoft SmartScreen_ är aktiverat:

![smartscreen_0](https://user-images.githubusercontent.com/2292809/120404034-c201f500-c345-11eb-9abd-670e927c4a36.png)

![smartscreen_1](https://user-images.githubusercontent.com/2292809/120404043-c4fce580-c345-11eb-945d-a5931bb5b721.png)


#### Distribuerad/Tyst installation/avinstallation
För att installera utan frågor ange /S som parameter till installationsprogrammet.
> WeekNumber_{VERSION}_Installer.exe /S

För att avinstallera utan frågor/gränssnitt anropa avinstallationsprogrammet med parameter /S.
> Uninstall WeekNumber.exe /S

## Hjälpavsnitt
Alla funktioner är tillgängliga genom att höger-klicka på applikationens ikon i Windows systemfält.
Om du inte ser applikationens ikon så tryck på taktecknet (^) vid systemfältet, klicka och håll inne musknappen medans du drar ikonen ned i systemfältet; då kommer ikonen fästas där och vara synlig.

### Högerklicks-meny:
- **Om WeekNumber** - _Visar versionsinformation_
- **Se om ny version finns** - _Kontrollerar om ny version finns, om så är fallet finns val att uppdatera_
- **Öppna applikationens webbsida** - _Visar applikationens webbsida_
- **Inställningar**
   - **Starta med Windows** - _Om ikryssad så startar applikationen automatiskt med Windows_
   - **Uppdatera automatiskt** - _Om ikryssad så uppdaterar applikationen automatiskt utan att du behöver göra något själv_
   - **Språk** - _Ändrar vilket språk som applikationen använder_
     - **English** - _Engelska_
     - **Svenska** 
   - **Applikationslogg**
      - **Använd applikationlogg** - _Om ikryssad så skriver applikationen till en loggfil_
      - **Visa applikationslogg** - _Om ovan är ikryssad så visas applikationens loggfil i en texteditor (teknisk loggfil)_
   - **Notiser**
      - **Visa uppstartsnotis** - _Om ikryssad så visar applikationen en uppstartsnotis_

      <img src="https://user-images.githubusercontent.com/2292809/121431229-6ce36600-c979-11eb-9ec4-37b77892e9fa.png" width="400">
      
      - **Visa ny veckonotis** - _Om ikryssad så visas en notis då aktuell vecka ändras_

      <img src="https://user-images.githubusercontent.com/2292809/121434601-d6fe0a00-c97d-11eb-83f3-ff553706cde9.png" width="400">

      - **Använd tysta notiser** - _Om ikryssad så spelas inte något ljud upp då denna applikation visas notiser_
   - **Använd små / stora aktivitetsfältsikoner** - _Skiftar mellan stora och små aktivitetsfältsikoner. Har du svårt att se veckonumret? Testa att använda stora aktivitetsfältsikoner_
   - **Kalender** - _Kalenderregler som appliationen använder sig av; vanligtvis tas detta från de regionella inställningar som är inställda på systemet, men de går alltså att justera manuellt här_
     - **'Första-dag-i-vecka'-regel** - _Kalenderregel, välj dag som en vecka startar på_
     - **'Årets-första-vecka'-regel** - _Ytterligare kalender-regel som anger vilket regelverk som ska nyttjas för årets första vecka_
   - **Ikon** - _Inställningar för applikationsikon_
     - **Ikonfärger** - _Möjlighet att anpassa appliktionens färger_
     - **Ikonupplösning** - _Möjlighet att anpassa WeekNumber-ikonens upplösning, om ikonen ser suddig ut kan en annan upplösning ge bättre ikonutseende_
     - **Grafikinställningar** - _Möjlighet att anpassa grafikalternativ som nyttjas då ikonen ritas av applikationen_
       - **Utjämningsläge**
       - **Kompositkvalitet**
       - **Interpolationsläge**
       - **Textkontrast**
     - **Spara ikon...** - _Sparar aktuell WeekNumber-ikon till en .ico fil_
- **Få veckonummer för datum...** - _Visa veckonummer för angivet datum_

  <img src="https://user-images.githubusercontent.com/2292809/121432297-c730f680-c97a-11eb-9530-005d5062039b.png" width="400">
- **Avsluta WeekNumber** - _Avslutar applikationen. Starta igen från Windows Startmeny_

## Donationer
*- WeekNumber är fullständigt gratis och öppen källkod. Donationer uppskattas!*

[![Donate](https://img.shields.io/badge/donate_via-paypal_or_card-blue)](https://www.paypal.com/donate?hosted_button_id=7PN65YXN64DBG) __⟵__ _**Tryck här för att donera!**_

[![ko-fi](https://user-images.githubusercontent.com/2292809/121091832-e8fb7380-c7ea-11eb-9c56-9768b1790ae5.png)](https://ko-fi.com/voltura) __⟵__ _**Tryck här för att köpa mig en kaffe!**_

## Statistik
[![Open issues](https://img.shields.io/github/issues/voltura/WeekNumber)](https://github.com/voltura/WeekNumber/issues)
[![Code Quality](https://img.shields.io/github/workflow/status/voltura/WeekNumber/CodeQL)]()
[![Website Status](https://img.shields.io/website?url=https%3A%2F%2Fvoltura.github.io%2FWeekNumber%2F)]()

[![Number of programming langauges](https://img.shields.io/github/languages/count/voltura/WeekNumber)]()
[![Top programming language](https://img.shields.io/github/languages/top/voltura/WeekNumber)]()
[![Code size](https://img.shields.io/github/languages/code-size/voltura/WeekNumber)]()
[![Number of repo forks](https://img.shields.io/github/forks/voltura/WeekNumber)]()
[![Number of repo stars](https://img.shields.io/github/stars/voltura/WeekNumber)]()
[![goto counter](https://img.shields.io/github/search/voltura/WeekNumber/goto)]()
[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fvoltura%2FWeekNumber%2Fhit-counter&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)]()
![GitHub last commit](https://img.shields.io/github/last-commit/voltura/WeekNumber?color=red)

![Visitors](https://estruyf-github.azurewebsites.net/api/VisitorHit?user=volturaf&repo=WeekNumber&countColorcountColor&countColor=%235690f2)
