package org.example.fripouilles.controller;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.stage.Stage;
import org.example.fripouilles.model.Contrat;
import org.example.fripouilles.model.Paie;
import org.example.fripouilles.service.ContratService;
import org.example.fripouilles.service.PaieService;

import java.io.IOException;
import java.time.LocalDate;
import java.util.List;

/**
 * Contrôleur pour générer une nouvelle fiche de paie
 */
public class GenererPaieController {
    
    @FXML
    private ComboBox<Contrat> contratComboBox;
    
    @FXML
    private ComboBox<Integer> moisComboBox;
    
    @FXML
    private ComboBox<Integer> anneeComboBox;
    
    @FXML
    private TextArea commentaireTextArea;
    
    @FXML
    private Button genererButton;
    
    @FXML
    private Button annulerButton;
    
    private final ContratService contratService;
    private final PaieService paieService;
    
    public GenererPaieController() {
        this.contratService = new ContratService();
        this.paieService = new PaieService();
    }
    
    @FXML
    private void initialize() {
        setupComboBoxes();
        loadContrats();
    }
    
    private void setupComboBoxes() {
        // Mois
        ObservableList<Integer> mois = FXCollections.observableArrayList();
        for (int m = 1; m <= 12; m++) {
            mois.add(m);
        }
        moisComboBox.setItems(mois);
        moisComboBox.setValue(LocalDate.now().getMonthValue());
        
        // Années
        int currentYear = LocalDate.now().getYear();
        ObservableList<Integer> annees = FXCollections.observableArrayList();
        for (int year = 2020; year <= currentYear + 1; year++) {
            annees.add(year);
        }
        anneeComboBox.setItems(annees);
        anneeComboBox.setValue(currentYear);
    }
    
    private void loadContrats() {
        try {
            List<Contrat> contrats = contratService.getAllContrats();
            
            // Filtrer les contrats actifs uniquement
            List<Contrat> contratsActifs = contrats.stream()
                .filter(c -> "ACTIF".equals(c.getStatut()))
                .toList();
            
            ObservableList<Contrat> contratList = FXCollections.observableArrayList(contratsActifs);
            contratComboBox.setItems(contratList);
            
        } catch (IOException e) {
            showError("Erreur", "Impossible de charger les contrats: " + e.getMessage());
        }
    }
    
    @FXML
    private void handleGenerer() {
        // Validation
        if (contratComboBox.getValue() == null) {
            showWarning("Champ manquant", "Veuillez sélectionner un contrat");
            return;
        }
        
        if (moisComboBox.getValue() == null) {
            showWarning("Champ manquant", "Veuillez sélectionner un mois");
            return;
        }
        
        if (anneeComboBox.getValue() == null) {
            showWarning("Champ manquant", "Veuillez sélectionner une année");
            return;
        }
        
        try {
            int contratId = contratComboBox.getValue().getId();
            int mois = moisComboBox.getValue();
            int annee = anneeComboBox.getValue();
            String commentaire = commentaireTextArea.getText().trim();
            
            // Générer la paie
            Paie paie = paieService.genererPaie(contratId, mois, annee,
                commentaire.isEmpty() ? null : commentaire);
            
            showInfo("Succès", "Fiche de paie générée avec succès!\n" +
                "Référence: #" + paie.getId() + "\n" +
                "Salaire net: " + String.format("%.2f €", paie.getSalaireNet()));
            
            // Fermer la fenêtre
            Stage stage = (Stage) genererButton.getScene().getWindow();
            stage.close();
            
        } catch (IOException e) {
            showError("Erreur", "Impossible de générer la fiche de paie: " + e.getMessage());
        }
    }
    
    @FXML
    private void handleAnnuler() {
        Stage stage = (Stage) annulerButton.getScene().getWindow();
        stage.close();
    }
    
    private void showError(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
    
    private void showWarning(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.WARNING);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
    
    private void showInfo(String title, String message) {
        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle(title);
        alert.setHeaderText(null);
        alert.setContentText(message);
        alert.showAndWait();
    }
}
