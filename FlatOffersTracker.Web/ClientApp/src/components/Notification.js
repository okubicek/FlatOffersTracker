import React, { Component } from 'react';

const noticationTypes = {
    OfferAdded: 'OfferAdded',
    OfferRemoved: 'OfferRemoved',
    PriceChanged: 'PriceChanged',
}

export class Notification extends Component {
    constructor(props) {
        super(props)
    }

    getNotificationText() {
        var type = noticationTypes[this.props.notificationType];
        switch (type) {
            case noticationTypes.OfferAdded:
                return 'Added';
            case noticationTypes.OfferRemoved:
                return 'Removed';
            case noticationTypes.PriceChanged:
                return 'Price changed';
        }
    }

    getNotificationClass() {
        var type = noticationTypes[this.props.notificationType];
        switch (type) {
            case noticationTypes.OfferAdded:
                return 'badge-success';
            case noticationTypes.OfferRemoved:
                return 'badge-danger';
            case noticationTypes.PriceChanged:
                return 'badge-warning';
        }
    }

    render() {
        return (
             this.props.notificationType != null
                ? < span className={'badge ' + this.getNotificationClass() + ' float-right'}> {this.getNotificationText()}</span >
                : null            
        );
    }
}