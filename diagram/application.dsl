workspace {
    model {
        console = element "Console" "" "Displays result" "Console"
        isun = element "isun" "" ".NET 6 Console Application" "SoftwareSystem"
        person = element "Person" "" "" "Person"
        settingsFile = element "appsettings.json" "" "Application Configuration File" "File"
        logFile = element "nlog.config" "" "Log Configuration File" "File"
        isunExternal = element "weather-api.isun.ch" "" "External API" "ExternalResource"
        dataDirectory = element "Data" "" "Windows Directory or Share" "Directory"
        logsDirectory = element "Logs" "" "Windows Directory or Share" "Directory"

        isunExternal -> isun "Provides City Weather"
        isunExternal -> isun "Provides Cities"
        isun -> isunExternal "Authentication"
        isun -> console "Writes"
        isun -> settingsFile "Loads Configuration"
        isun -> logFile "Loads Configuration"
        person -> console "Reads"
        person -> settingsFile "Configures"
        person -> logFile "Configures"
        person -> logsDirectory "Views"
        person -> dataDirectory "Views"
        person -> isun "Launches"
        isun -> dataDirectory "Saves Weather Data by City"
        isun -> logsDirectory "Logs"
    }

    views {
        custom "isun" "Use Case" {
            include *
            autoLayout
        }

        styles {

            element "Database" {
                shape Cylinder
                background #1168bd
                color #ffffff
            }

            element "Person" {
                shape Person
                background #1168bd
                color #ffffff
            }

            element "SoftwareSystem" {
                shape RoundedBox
                background #08427b
                color #ffffff
            }

            element "Directory" {
                shape Folder
                background #1168bd
                color #ffffff
            }

            element "ExternalResource" {
                shape Component
                background #964B00
                color #ffffff
            }

            element "Console" {
                shape WebBrowser
                background #333333
                color #ffffff
            }

            element "File" {
                shape Box
                background #1168bd
                color #ffffff
            }
        }
    }
}
