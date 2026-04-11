package org.example.fripouilles.controller;

import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.layout.VBox;
import javafx.scene.shape.Rectangle;
import javafx.stage.Stage;
import org.example.fripouilles.model.Paie;

public class PaieDetailController {

    @FXML
    private VBox root;

    @FXML
    private Label idLabel;

    @FXML
    private Label contratLabel;

    @FXML
    private Label enfantLabel;

    @FXML
    private Label assistantLabel;

    @FXML
    private Label parentLabel;

    @FXML
    private Label periodeLabel;

    @FXML
    private Label heuresNormalesLabel;

    @FXML
    private Label heuresMajoreesLabel;

    @FXML
    private Label totalHeuresLabel;

    @FXML
    private Label salaireBrutLabel;

    @FXML
    private Label chargesPatronalesLabel;

    @FXML
    private Label chargesSalarialesLabel;

    @FXML
    private Label salaireNetLabel;

    @FXML
    private Label priseEnChargeCAFLabel;

    @FXML
    private TextArea commentaireTextArea;

    @FXML
    private Label dateCreationLabel;

    private Paie paie;

    @FXML
    public void initialize() {
        Platform.runLater(() -> {
            Rectangle clip = new Rectangle();
            clip.widthProperty().bind(root.widthProperty());
            clip.heightProperty().bind(root.heightProperty());
            clip.setArcWidth(20);
            clip.setArcHeight(20);
            root.setClip(clip);
        });
    }

    public void setPaie(Paie paie) {
        this.paie = paie;
        displayPaieDetails();
    }

    private void displayPaieDetails() {
        if (paie == null) return;

        idLabel.setText(String.valueOf(paie.getId()));
        contratLabel.setText("Contrat #" + paie.getContratId());

        if (paie.getContrat() != null) {
            if (paie.getContrat().getEnfant() != null) {
                var enfant = paie.getContrat().getEnfant();
                enfantLabel.setText(enfant.getPrenom() + " " + enfant.getNom());
            }

            if (paie.getContrat().getAssistant() != null &&
                    paie.getContrat().getAssistant().getUtilisateur() != null) {
                var user = paie.getContrat().getAssistant().getUtilisateur();
                assistantLabel.setText(user.getNom() + " " + user.getPrenom());
            }

            if (paie.getContrat().getParent() != null &&
                    paie.getContrat().getParent().getUtilisateur() != null) {
                var user = paie.getContrat().getParent().getUtilisateur();
                parentLabel.setText(user.getNom() + " " + user.getPrenom());
            }
        }

        periodeLabel.setText(String.format("%02d/%d", paie.getMois(), paie.getAnnee()));
        heuresNormalesLabel.setText(String.format("%.2f h", paie.getHeuresNormales()));
        heuresMajoreesLabel.setText(String.format("%.2f h", paie.getHeuresMajorees()));
        totalHeuresLabel.setText(String.format("%.2f h", paie.getTotalHeures()));

        salaireBrutLabel.setText(String.format("%.2f €", paie.getSalaireBrut()));
        chargesPatronalesLabel.setText(String.format("%.2f €", paie.getChargesPatronales()));
        chargesSalarialesLabel.setText(String.format("%.2f €", paie.getChargesSalariales()));
        salaireNetLabel.setText(String.format("%.2f €", paie.getSalaireNet()));

        priseEnChargeCAFLabel.setText(paie.getPriseEnChargeCAF() != null
                ? String.format("%.2f €", paie.getPriseEnChargeCAF())
                : "N/A");

        commentaireTextArea.setText(
                (paie.getCommentaire() != null && !paie.getCommentaire().isEmpty())
                        ? paie.getCommentaire()
                        : "Aucun commentaire"
        );

        if (paie.getCreatedAt() != null) {
            dateCreationLabel.setText(paie.getCreatedAt());
        }
    }

    @FXML
    private void handleClose() {
        Stage stage = (Stage) idLabel.getScene().getWindow();
        stage.close();
    }
}