package org.example.fripouilles.controller;

import javafx.application.Platform;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.StackPane;
import javafx.scene.paint.Color;
import javafx.scene.shape.Rectangle;
import javafx.stage.Modality;
import javafx.stage.Stage;
import javafx.stage.StageStyle;
import org.example.fripouilles.model.Paie;
import org.example.fripouilles.service.AuthService;
import org.example.fripouilles.service.ContratService;
import org.example.fripouilles.service.PaieService;

import java.io.IOException;
import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

public class MainController {

    @FXML
    private BorderPane root;

    @FXML
    private StackPane overlay;

    @FXML
    private TableView<Paie> paieTableView;

    @FXML
    private TableColumn<Paie, Integer> idColumn;

    @FXML
    private TableColumn<Paie, String> contratColumn;

    @FXML
    private TableColumn<Paie, String> assistantColumn;

    @FXML
    private TableColumn<Paie, String> parentColumn;

    @FXML
    private TableColumn<Paie, String> periodeColumn;

    @FXML
    private TableColumn<Paie, Double> heuresColumn;

    @FXML
    private TableColumn<Paie, Double> salaireNetColumn;

    @FXML
    private ComboBox<Integer> anneeComboBox;

    @FXML
    private ComboBox<Integer> moisComboBox;

    @FXML
    private TextField searchField;

    private Stage loginStage;

    private final PaieService paieService;
    private final AuthService authService;
    private ObservableList<Paie> paieList;

    public MainController() {
        this.paieService = new PaieService();
        new ContratService();
        this.authService = new AuthService();
    }

    public void setLoginStage(Stage loginStage) {
        this.loginStage = loginStage;
    }

    @FXML
    private void initialize() {
        try {
            Platform.runLater(() -> {
                Rectangle clip = new Rectangle();
                clip.widthProperty().bind(root.widthProperty());
                clip.heightProperty().bind(root.heightProperty());
                clip.setArcWidth(20);
                clip.setArcHeight(20);
                root.setClip(clip);
            });
            setupTableColumns();
            setupFilters();
            setupDoubleClickHandler();
            loadPaies();
        } catch (Exception e) {
            e.printStackTrace();
            showError("Erreur d'initialisation", e.getMessage());
        }
    }

    private void setupDoubleClickHandler() {
        paieTableView.setOnMouseClicked(event -> {
            if (event.getClickCount() == 2 && paieTableView.getSelectionModel().getSelectedItem() != null) {
                handleViewDetails();
            }
        });
    }

    private void setupTableColumns() {
        idColumn.setCellValueFactory(cellData ->
                new SimpleIntegerProperty(cellData.getValue().getId()).asObject());

        contratColumn.setCellValueFactory(cellData -> {
            Paie paie = cellData.getValue();
            if (paie.getContrat() != null && paie.getContrat().getEnfant() != null) {
                return new SimpleStringProperty("Contrat #" + paie.getContratId() + " - " +
                        paie.getContrat().getEnfant().getPrenom() + " " +
                        paie.getContrat().getEnfant().getNom());
            }
            return new SimpleStringProperty("Contrat #" + paie.getContratId());
        });

        assistantColumn.setCellValueFactory(cellData -> {
            Paie paie = cellData.getValue();
            if (paie.getContrat() != null && paie.getContrat().getAssistant() != null
                    && paie.getContrat().getAssistant().getUtilisateur() != null) {
                var user = paie.getContrat().getAssistant().getUtilisateur();
                return new SimpleStringProperty(user.getNom() + " " + user.getPrenom());
            }
            return new SimpleStringProperty("-");
        });

        parentColumn.setCellValueFactory(cellData -> {
            Paie paie = cellData.getValue();
            if (paie.getContrat() != null && paie.getContrat().getParent() != null
                    && paie.getContrat().getParent().getUtilisateur() != null) {
                var user = paie.getContrat().getParent().getUtilisateur();
                return new SimpleStringProperty(user.getNom() + " " + user.getPrenom());
            }
            return new SimpleStringProperty("-");
        });

        periodeColumn.setCellValueFactory(cellData -> {
            Paie paie = cellData.getValue();
            return new SimpleStringProperty(String.format("%02d/%d", paie.getMois(), paie.getAnnee()));
        });

        heuresColumn.setCellValueFactory(cellData ->
                new SimpleDoubleProperty(cellData.getValue().getTotalHeures()).asObject());

        salaireNetColumn.setCellValueFactory(cellData ->
                new SimpleDoubleProperty(cellData.getValue().getSalaireNet()).asObject());

        salaireNetColumn.setCellFactory(column -> new TableCell<>() {
            @Override
            protected void updateItem(Double item, boolean empty) {
                super.updateItem(item, empty);
                if (empty || item == null) {
                    setText(null);
                } else {
                    setText(String.format("%.2f €", item));
                }
            }
        });
    }

    private void setupFilters() {
        int currentYear = LocalDate.now().getYear();
        ObservableList<Integer> annees = FXCollections.observableArrayList();
        annees.add(null);
        for (int year = 2020; year <= currentYear + 1; year++) {
            annees.add(year);
        }
        anneeComboBox.setItems(annees);
        anneeComboBox.setValue(currentYear);
        anneeComboBox.valueProperty().addListener((obs, oldVal, newVal) -> loadPaies());

        ObservableList<Integer> mois = FXCollections.observableArrayList();
        mois.add(null);
        for (int m = 1; m <= 12; m++) {
            mois.add(m);
        }
        moisComboBox.setItems(mois);
        moisComboBox.setValue(null);
        moisComboBox.valueProperty().addListener((obs, oldVal, newVal) -> loadPaies());

        searchField.textProperty().addListener((obs, oldVal, newVal) -> filterPaies(newVal));
    }

    @FXML
    private void handleRefresh() {
        loadPaies();
    }

    @FXML
    private void handleGenererPaie() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/org/example/fripouilles/generer-paie-view.fxml"));
            Parent fxmlRoot = loader.load();

            Scene scene = new Scene(fxmlRoot, 800, 500);
            scene.setFill(Color.TRANSPARENT);

            Stage stage = new Stage();
            stage.initStyle(StageStyle.TRANSPARENT);
            stage.setTitle("Générer une fiche de paie");
            stage.setScene(scene);
            stage.initModality(Modality.APPLICATION_MODAL);

            overlay.setVisible(true);
            stage.setOnHidden(e -> overlay.setVisible(false));

            stage.showAndWait();
            loadPaies();
        } catch (IOException e) {
            showError("Erreur", "Impossible d'ouvrir la fenêtre de génération: " + e.getMessage());
        }
    }

    @FXML
    private void handleViewDetails() {
        Paie selectedPaie = paieTableView.getSelectionModel().getSelectedItem();
        if (selectedPaie == null) {
            showWarning("Aucune sélection", "Veuillez sélectionner une fiche de paie");
            return;
        }

        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/org/example/fripouilles/paie-detail-view.fxml"));
            Parent fxmlRoot = loader.load();

            PaieDetailController controller = loader.getController();
            controller.setPaie(selectedPaie);

            Scene scene = new Scene(fxmlRoot, 700, 600);
            scene.setFill(Color.TRANSPARENT);

            Stage stage = new Stage();
            stage.initStyle(StageStyle.TRANSPARENT);
            stage.setTitle("Détails de la fiche de paie");
            stage.setScene(scene);
            stage.initModality(Modality.APPLICATION_MODAL);

            overlay.setVisible(true);
            stage.setOnHidden(e -> overlay.setVisible(false));

            stage.show();
        } catch (IOException e) {
            showError("Erreur", "Impossible d'ouvrir les détails: " + e.getMessage());
        }
    }

    @FXML
    private void handleDelete() {
        Paie selectedPaie = paieTableView.getSelectionModel().getSelectedItem();
        if (selectedPaie == null) {
            showWarning("Aucune sélection", "Veuillez sélectionner une fiche de paie");
            return;
        }

        Alert confirmation = new Alert(Alert.AlertType.CONFIRMATION);
        confirmation.setTitle("Confirmation");
        confirmation.setHeaderText("Supprimer la fiche de paie");
        confirmation.setContentText("Êtes-vous sûr de vouloir supprimer cette fiche de paie ?");

        Optional<ButtonType> result = confirmation.showAndWait();
        if (result.isPresent() && result.get() == ButtonType.OK) {
            try {
                paieService.deletePaie(selectedPaie.getId());
                showInfo("Succès", "Fiche de paie supprimée avec succès");
                loadPaies();
            } catch (IOException e) {
                showError("Erreur", "Impossible de supprimer la fiche de paie: " + e.getMessage());
            }
        }
    }

    @FXML
    private void handleLogout() {
        authService.logout();

        Stage mainStage = (Stage) paieTableView.getScene().getWindow();
        loginStage.show();
        mainStage.close();
    }

    private void loadPaies() {
        try {
            Integer annee = anneeComboBox.getValue();
            Integer mois = moisComboBox.getValue();

            List<Paie> paies = paieService.getAllPaies(annee, mois);
            paieList = FXCollections.observableArrayList(paies);
            filterPaies(searchField.getText());
        } catch (IOException e) {
            showError("Erreur", "Impossible de charger les fiches de paie: " + e.getMessage());
        }
    }

    private void filterPaies(String searchText) {
        if (searchText == null || searchText.isEmpty()) {
            paieTableView.setItems(paieList);
        } else {
            ObservableList<Paie> filtered = paieList.filtered(paie ->
                    paie.getContrat() != null &&
                            paie.getContrat().getEnfant() != null &&
                            (paie.getContrat().getEnfant().getPrenom().toLowerCase().contains(searchText.toLowerCase()) ||
                                    paie.getContrat().getEnfant().getNom().toLowerCase().contains(searchText.toLowerCase()))
            );
            paieTableView.setItems(filtered);
        }
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