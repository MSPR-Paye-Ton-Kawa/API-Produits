# PS-2024-MSPR-Paye-Ton-Kawa

## 📚 Projet Scolaire | MSPR

Juin-Septembre 2024

Groupe : Juliette, Flavien, Yasmine & Colas

### 📌 Consignes du projet : 

CERTIFICATION PROFESSIONNELLE EXPERT EN INFORMATIQUE ET SYSTEME D’INFORMATION

BLOC 4 – Concevoir et développer des solutions applicatives métier et spécifiques (mobiles, embarquées et ERP)

Cahier des Charges de la MSPR « Conception d’une solution applicative en adéquation avec l’environnement technique étudié


### 🐱 Notre projet :

Ce repos est destiné à l'API Produits.

📦 Table Products :

- ProductId (int, Primary Key, Auto-increment) : Identifiant unique du produit.

- Name (varchar, not null) : Nom du produit.

- Description (text, null) : Description du produit.

- Price (decimal, not null) : Prix du produit.

- StockQuantity (int, not null) : Quantité en stock.

- CategoryId (int, Foreign Key, not null) : Identifiant de la catégorie associée.

- SupplierId (int, Foreign Key, not null) : Identifiant du fournisseur.

- CreatedAt (datetime, not null, default current timestamp) : Date de création du produit.

- UpdatedAt (datetime, not null, default current timestamp on update current timestamp) : Date de la dernière mise à jour du produit.


📦 Table Categories :

- CategoryId (int, Primary Key, Auto-increment) : Identifiant unique de la catégorie.

- CategoryName (varchar, not null) : Nom de la catégorie.

- CategoryType (varchar, null) : Type de la catégorie.


📦 Table Suppliers :

- SupplierId (int, Primary Key, Auto-increment) : Identifiant unique du fournisseur.

- SupplierName (varchar, not null) : Nom du fournisseur.

- ContactEmail (varchar, null) : Email de contact du fournisseur.


Commandes Docker :

docker build -t apiproduits .

docker run -p 8080:80 --name apiproduits apiproduits:latest


### 📎 Branches :

- main : Solution finale, prod.
  
- dev : Solution fonctionnelle en dev.
  
- hotfix : Correction de bugs et autres.

- release : Solution fonctionnelle de dev à prod.

- feature-db : Développement lié à la base de données.

- feature-tests : Développement des tests.

- feature-owasp-dependency-check : Développement de la partie sécurité.

- feature-broker : Développement de la partie message broker.

- feature-docker : Développement de la partie Docker.

- bugfix-* : Correction de bugs.


### 💻 Applications et langages utilisés :

- C#
- Visual Studio
- Docker



## 🌸 Merci !
© J-IFT
