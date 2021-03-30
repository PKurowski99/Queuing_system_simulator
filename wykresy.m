close all
clear all
%% Setup the Import Options and import the data
opts = delimitedTextImportOptions("NumVariables", 2);

% Specify range and delimiter
opts.DataLines = [1, Inf];
opts.Delimiter = "      ";

% Specify column names and types
opts.VariableNames = ["time", "value"];
opts.VariableTypes = ["double", "double"];

% Specify file level properties
opts.ExtraColumnsRule = "ignore";
opts.EmptyLineRule = "read";
opts.ConsecutiveDelimitersRule = "join";

% Specify variable properties
opts = setvaropts(opts, "time", "TrimNonNumeric", true);
opts = setvaropts(opts, ["time", "value"], "DecimalSeparator", ",");
opts = setvaropts(opts, "time", "ThousandsSeparator", ".");

% Import the data
kolejki0 = readtable("logs\kolejki_0.txt", opts);
kolejki1 = readtable("logs\kolejki_1.txt", opts);
zajetosc0 = readtable("logs\zajetosc_0.txt", opts);
zajetosc1 = readtable("logs\zajetosc_1.txt", opts);
opoznienia0 = readtable("logs\opoznienia_0.txt", opts);
opoznienia1 = readtable("logs\opoznienia_1.txt", opts);

m0 = max(kolejki0.value);
m1 = max(kolejki1.value);
m=max(m0,m1);

n0 = max(opoznienia0.value);
n1 = max(opoznienia1.value);
n=max(n0,n1);

figure(1)

    set(gcf, 'Units', 'Normalized', 'OuterPosition', [0, 0.04, 1, 0.96]);
    
    subplot(2,1,1); 
        plot(kolejki0.time,kolejki0.value,'Color','#cf1d70','LineWidth',1.5)
        ylim([0 m])
        ax = gca;
        ax.FontSize = 12;
        title("Stan kolejki w wêŸle nr 0 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("Liczba pakietów w kolejce", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);
        hold on;
        
    subplot(2,1,2); 
        plot(kolejki1.time,kolejki1.value,'Color','#172a78','LineWidth',1.5)
        ylim([0 m])
        ax = gca;
        ax.FontSize = 12;
        title("Stan kolejki w wêŸle nr 1 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("Liczba pakietów w kolejce", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);
        hold on;

figure(2)

    set(gcf, 'Units', 'Normalized', 'OuterPosition', [0, 0.04, 1, 0.96]);

    subplot(2,1,1); 
        plot(zajetosc0.time,zajetosc0.value,'Color','#cf1d70','LineWidth',1.5)
        hold on;
        ylim([0 1.2])
        yticks([0 1])
        yticklabels({'wolny','zajety'})
        ax = gca;
        ax.FontSize = 12;
        title("Stan serwera w wêŸle nr 0 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("Stan serwera", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);
    subplot(2,1,2); 
        plot(zajetosc1.time,zajetosc1.value,'Color','#172a78','LineWidth',1.5)
        hold on;
        ylim([0 1.2])
        yticks([0 1])
        yticklabels({'wolny','zajety'})
        ax = gca;
        ax.FontSize = 12;
        title("Stan serwera w wêŸle nr 1 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("Stan serwera", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);

figure(3)
    set(gcf, 'Units', 'Normalized', 'OuterPosition', [0, 0.04, 1, 0.96]);
    subplot(2,1,1); 
        plot(opoznienia0.time,opoznienia0.value,'Color','#cf1d70','LineWidth',1.5)
        hold on;
        ax = gca;
        ax.FontSize = 12;
        title("OpóŸnienie obs³ugiwanego pakietu w wêŸle nr 0 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("OpóŸnienie[ms]", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);
        ylim([0 n])
    subplot(2,1,2); 
        plot(opoznienia1.time,opoznienia1.value,'Color','#172a78','LineWidth',1.5)
        hold on;
        ax = gca;
        ax.FontSize = 12;
        title("OpóŸnienie obs³ugiwanego pakietu w wêŸle nr 1 w zale¿noœci od czasu", 'FontSize',14);
        ylabel("OpóŸnienie[ms]", 'FontSize',14);
        xlabel('Czas[ms]', 'FontSize',14);
        ylim([0 n])
%% Clear temporary variables
clear opts