import React, { Component } from 'react';
import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";
import "./react-datepicker.css";

export class DateBox extends Component {
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(d, e) {
        e.target.name = this.props.name;
        e.target.value = d;
        this.props.onChange(e.target);
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <DatePicker
                    selected={this.props.value}
                    onChange={this.handleChange}
                    className="form-control"
                    name={this.props.name}
                />
            </div>
        );
    }
}